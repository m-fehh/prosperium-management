using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Transactions.Dto;
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
        public async Task<PagedResultDto<GetTransactionForViewDto>> GetAllAsync(GetAllTransactionFilter input)
        {
            var allTransaction = _transactionRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Filter), x => false ||
                 x.Description.ToLower().Trim().Contains(input.Filter.ToLower().Trim()));

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
