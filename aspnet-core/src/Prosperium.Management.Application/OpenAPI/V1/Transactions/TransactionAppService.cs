using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
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
        public async Task CreateAsync(CreateTransactionDto input)
        {
            var transaction = ObjectMapper.Map<Transaction>(input);

            if (transaction.TransactionType == TransactionType.Gastos || transaction.TransactionType == TransactionType.Transferência)
            {
                transaction.ExpenseValue = -Math.Abs(transaction.ExpenseValue);

                if(transaction.TransactionType == TransactionType.Transferência)
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


        [HttpGet("{id}")]
        public async Task<TransactionDto> GetByIdAsync(long id)
        {
            Transaction transaction = await _transactionRepository.GetAsync(id);

            if (transaction == null)
            {
                throw new UserFriendlyException("A transação não foi encontrada.");
            }

            return ObjectMapper.Map<TransactionDto>(transaction);
        }

        [HttpGet]
        [Route("list-transaction")]
        public async Task<List<TransactionDto>> GetAllListAsync()
        {
            List<Transaction> allTransactions = await _transactionRepository
                .GetAll().Include(x => x.Categories)
                    .ThenInclude(x => x.Subcategories)
                .Include(x => x.Account)
                    .ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard)
                    .ThenInclude(x => x.FlagCard).ToListAsync();
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
            var allTransaction = _transactionRepository.GetAll()
                .Include(x => x.Categories)
                    .ThenInclude(x => x.Subcategories)
                .Include(x => x.Account)
                    .ThenInclude(x => x.Bank)
                .Include(x => x.CreditCard)
                    .ThenInclude(x => x.FlagCard)
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Description.ToLower().Trim().Contains(input.Filter.ToLower().Trim()));

            // Filtros avançados
            if (!string.IsNullOrEmpty(input.FilteredAccounts))
            {
                input.MonthYear = null;

                var accountIds = input.FilteredAccounts.Split(',').Select(id => long.Parse(id)).ToList();
                allTransaction = allTransaction.Where(x => accountIds.Contains(x.AccountId.Value));
            }

            if (!string.IsNullOrEmpty(input.filteredCards))
            {
                input.MonthYear = null;

                var cardIds = input.filteredCards.Split(',').Select(id => long.Parse(id)).ToList();
                allTransaction = allTransaction.Where(x => cardIds.Contains(x.CreditCardId.Value));
            }

            if (!string.IsNullOrEmpty(input.FilteredCategories))
            {
                input.MonthYear = null;

                var categoryIds = input.FilteredCategories.Split(',').Select(id => long.Parse(id)).ToList();
                allTransaction = allTransaction.Where(x => categoryIds.Contains(x.CategoryId));
            }

            if (!string.IsNullOrEmpty(input.FilteredTags))
            {
                input.MonthYear = null;

                var tagsIds = input.FilteredTags.Split(',').Select(id => long.Parse(id)).ToList();
                allTransaction = allTransaction.Where(x => x.Tags.Any(tag => tagsIds.Contains(tag.Id)));
            }

            if (!string.IsNullOrEmpty(input.FilteredTypes))
            {
                input.MonthYear = null;

                var transactionTypes = input.FilteredTypes.Split(',').Select(type => Enum.Parse(typeof(TransactionType), type)).Cast<TransactionType>().ToList();
                allTransaction = allTransaction.Where(x => transactionTypes.Contains(x.TransactionType));
            }



            // Filtro por calendário
            if (!string.IsNullOrEmpty(input.MonthYear))
            {
                var parts = input.MonthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                allTransaction = allTransaction.Where(x => x.Date.Month == month && x.Date.Year == year);
            }

            var pagedAndFilteredTransaction = allTransaction.OrderBy(input.Sorting ?? "date desc").PageBy(input);

            var transactions = from o in pagedAndFilteredTransaction
                               select new GetTransactionForViewDto()
                               {
                                   Transaction = ObjectMapper.Map<TransactionDto>(o),
                               };

            int totalCount = await allTransaction.CountAsync();
            return new PagedResultDto<GetTransactionForViewDto>
            (
                totalCount,
                await transactions.ToListAsync()
            );
        }

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
