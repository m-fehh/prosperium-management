using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    [Route("v1/transactions")]
    public class TransactionAppService : ManagementAppServiceBase, ITransactionAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Transaction, long> _transactionRepository;
        private readonly IRepository<Category, long> _categoryRepository;

        public TransactionAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<Transaction, long> transactionRepository, IRepository<Category, long> categoryRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Route("CreateAtomicTransaction")]
        public async Task CreateAtomicTransactionAsync(List<Transaction> input)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                await _transactionRepository.InsertRangeAsync(input);
                uow.Complete();
            }
        }

        [HttpPost]
        public async Task CreateAsync(CreateTransactionDto input)
        {
            var transaction = ObjectMapper.Map<Transaction>(input);
            transaction.Origin = AccountConsts.AccountOrigin.Manual;
            transaction.Tags = $"Integração manual,{input.Tags}";

            if (transaction.TransactionType == TransactionType.Gastos || transaction.TransactionType == TransactionType.Transferência)
            {
                transaction.ExpenseValue = -Math.Abs(transaction.ExpenseValue);

                if (transaction.TransactionType == TransactionType.Transferência)
                {
                    var categoryTransfer = await _categoryRepository.FirstOrDefaultAsync(x => x.Name.Equals("Transferência"));
                    transaction.CategoryId = categoryTransfer.Id;
                }
            }

            // Se for crédito a vista
            if (input.PaymentType == PaymentType.Crédito && input.PaymentTerm == PaymentTerms.Imediatamente)
            {
                transaction.CurrentInstallment = "1/1";
                await _transactionRepository.InsertAsync(transaction);
            }

            // Se for crédito parcelado
            if (input.PaymentType == PaymentType.Crédito && input.PaymentTerm == PaymentTerms.Parcelado)
            {
                decimal installmentValue = transaction.ExpenseValue / transaction.Installments.Value;

                for (int i = 1; i <= transaction.Installments; i++)
                {
                    var parcelTransaction = ObjectMapper.Map<Transaction>(input);

                    if (i > 1)
                    {
                        parcelTransaction.Date = transaction.Date.AddMonths(i - 1);
                    }

                    parcelTransaction.ExpenseValue = installmentValue;
                    parcelTransaction.CurrentInstallment = $"{i}/{transaction.Installments}";

                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        await _transactionRepository.InsertAsync(parcelTransaction);
                        uow.Complete();
                    }
                }
            }

            // Se for crédito recorrente
            if (input.PaymentType == PaymentType.Crédito && input.PaymentTerm == PaymentTerms.Recorrente) { }

            // Se for débito a vista
            if (input.PaymentType == PaymentType.Débito && input.PaymentTerm == PaymentTerms.Imediatamente)
            {
                await _transactionRepository.InsertAsync(transaction);
            }

            // Se for crédito recorrente
            if (input.PaymentType == PaymentType.Débito && input.PaymentTerm == PaymentTerms.Recorrente) { }
        }

        [HttpGet]
        [Route("get-by-id-async")]
        public async Task<TransactionDto> GetByIdAsync(long id)
        {
            Transaction transaction = await _transactionRepository.GetAll()
                .Include(x => x.Categories).ThenInclude(x => x.Subcategories)
                .Include(x => x.Account).ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard).ThenInclude(x => x.FlagCard)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return ObjectMapper.Map<TransactionDto>(transaction);
        }

        [HttpGet]
        [Route("list-transaction-per-itemId")]
        public async Task<List<string>> GetAllTransactionPerItemIdAsync(int tenantId, string pluggyItemId, bool isCreditCard)
        {
            var query = _transactionRepository
                .GetAll()
                .Where(x => x.TenantId == tenantId)
                .Where(x => (isCreditCard) ? x.CreditCard.PluggyItemId.Equals(pluggyItemId) : x.Account.PluggyItemId.Equals(pluggyItemId));

            var selectedColumnsQuery = query
                .Select(x => x.PluggyTransactionId);

            var allTransactionIds = await selectedColumnsQuery.ToListAsync();
            return allTransactionIds;
        }


        [HttpGet]
        [Route("list-transaction-per-month")]
        public async Task<List<TransactionDto>> GetAllListPerMonthAsync(string monthYear, string filteredAccounts, string filteredCards, string filteredCategories)
        {
            var parts = monthYear.Split('/');
            var month = int.Parse(parts[0]);
            var year = int.Parse(parts[1]);

            var query = _transactionRepository.GetAll()
                .Include(x => x.Categories).ThenInclude(x => x.Subcategories)
                .Include(x => x.Account).ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard).ThenInclude(x => x.FlagCard)
                .Where(x => x.Account.AccountType != AccountConsts.AccountType.Crédito)
                .Where(x => x.Date.Month == month && x.Date.Year == year);

            if (!string.IsNullOrEmpty(filteredAccounts))
            {
                var accountIds = filteredAccounts.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => accountIds.Contains(x.AccountId.Value));
            }

            if (!string.IsNullOrEmpty(filteredCards))
            {
                var cardIds = filteredCards.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => cardIds.Contains(x.CreditCardId.Value));
            }

            if (!string.IsNullOrEmpty(filteredCategories))
            {
                var categoryIds = filteredCategories.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => categoryIds.Contains(x.CategoryId));
            }

            var allTransactions = await query.ToListAsync();

            return ObjectMapper.Map<List<TransactionDto>>(allTransactions);
        }

        [HttpGet]
        [Route("list-transaction-per-account")]
        public async Task<List<TransactionDto>> GetAllTransactionPerAccount(long accountId)
        {
            List<Transaction> allTransactions = await _transactionRepository
                .GetAll().Include(x => x.Categories)
                    .ThenInclude(x => x.Subcategories)
                .Include(x => x.Account)
                    .ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard)
                    .ThenInclude(x => x.FlagCard).Where(x => x.AccountId == accountId)
                .ToListAsync();
            return ObjectMapper.Map<List<TransactionDto>>(allTransactions);
        }

        [HttpGet]
        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllAsync(GetAllTransactionFilter input)
        {
            var query = _transactionRepository.GetAll()
                .Include(x => x.Categories).ThenInclude(x => x.Subcategories)
                .Include(x => x.Account).ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard).ThenInclude(x => x.FlagCard)
                .Where(x => x.Account.AccountType != AccountConsts.AccountType.Crédito);

            // Filtro por descrição
            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(x => x.Description.ToLower().Trim().Contains(input.Filter.ToLower().Trim()));
            }

            // Filtros avançados
            query = ApplyAdvancedFilters(query, input);

            // Filtro por calendário
            query = ApplyCalendarFilter(query, input);

            var pagedAndFilteredTransaction = query.OrderBy(input.Sorting ?? "date desc").PageBy(input);

            var transactions = pagedAndFilteredTransaction.Select(o => new GetTransactionForViewDto
            {
                Transaction = ObjectMapper.Map<TransactionDto>(o),
            });

            int totalCount = await query.CountAsync();
            return new PagedResultDto<GetTransactionForViewDto>(totalCount, await transactions.ToListAsync());
        }

        #region PRIVATE METHODS 

        private IQueryable<Transaction> ApplyAdvancedFilters(IQueryable<Transaction> query, GetAllTransactionFilter input)
        {
            if (!string.IsNullOrEmpty(input.FilteredAccounts))
            {
                var accountIds = input.FilteredAccounts.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => accountIds.Contains(x.AccountId.Value));
            }

            if (!string.IsNullOrEmpty(input.filteredCards))
            {
                var cardIds = input.filteredCards.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => cardIds.Contains(x.CreditCardId.Value));
            }

            if (!string.IsNullOrEmpty(input.FilteredCategories))
            {
                var categoryIds = input.FilteredCategories.Split(',').Select(id => long.Parse(id)).ToList();
                query = query.Where(x => categoryIds.Contains(x.CategoryId));
            }

            if (input.FilteredExpense.HasValue && input.FilteredExpense == true)
            {
                query = query.Where(x => x.TransactionType == TransactionType.Gastos);
            }

            if (input.FilteredRecipes.HasValue && input.FilteredRecipes == true)
            {
                query = query.Where(x => x.TransactionType == TransactionType.Ganhos);
            }

            return query;
        }

        private IQueryable<Transaction> ApplyCalendarFilter(IQueryable<Transaction> query, GetAllTransactionFilter input)
        {
            if (!string.IsNullOrEmpty(input.MonthYear))
            {
                var parts = input.MonthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);
                query = query.Where(x => x.Date.Month == month && x.Date.Year == year);
            }

            return query;
        }
        #endregion

        [HttpPut]
        public async Task UpdateAsync(TransactionDto input)
        {
            Transaction existingTransaction = await _transactionRepository.FirstOrDefaultAsync(input.Id);

            if (existingTransaction == null)
            {
                throw new UserFriendlyException("A transação não foi encontrada.");
            }

            ObjectMapper.Map(input, existingTransaction);

            await _transactionRepository.UpdateAsync(existingTransaction);
        }

        [HttpDelete("id")]
        public async Task DeleteAsync(long id)
        {
            Transaction searchTransaction = await _transactionRepository.FirstOrDefaultAsync(id);

            if (searchTransaction == null)
            {
                throw new UserFriendlyException("A transação não foi encontrada.");
            }

            await _transactionRepository.DeleteAsync(searchTransaction);
        }

    }
}
