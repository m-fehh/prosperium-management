using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    [Route("v1/transactions")]
    public class TransactionAppService : ManagementAppServiceBase, ITransactionAppService
    {
        private readonly IRepository<Transaction, long> _transactionRepository;

        public TransactionAppService(IRepository<Transaction, long> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public async Task CreateAsync(TransactionDto input)
        {
            Transaction transaction = ObjectMapper.Map<Transaction>(input);
            await _transactionRepository.InsertAsync(transaction);
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
            List<Transaction> allTransactions = await _transactionRepository.GetAllListAsync();
            return ObjectMapper.Map<List<TransactionDto>>(allTransactions);
        }

        [HttpGet]
        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllAsync(GetAllTransactionFilter input)
        {
            var allTransaction = _transactionRepository.GetAll()
                .Include(x => x.Categories)
                    .ThenInclude(x => x.Subcategories)
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Description.ToLower().Trim().Contains(input.Filter.ToLower().Trim()));

            if (!string.IsNullOrEmpty(input.MonthYear))
            {
                var parts = input.MonthYear.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                allTransaction = allTransaction.Where(x => x.Date.Month == month && x.Date.Year == year);
            }

            var pagedAndFilteredTransaction = allTransaction.OrderBy(input.Sorting ?? "id asc").PageBy(input);

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
