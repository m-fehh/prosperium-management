using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Flags;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    [Route("v1/cards")]
    public class CreditCardAppService : ManagementAppServiceBase, ICreditCardAppService
    {
        private readonly IRepository<CreditCard, long> _creditCardRepository;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IRepository<FlagCard, long> _flagCardRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CreditCardAppService(IRepository<CreditCard, long> creditCardRepository, ITransactionAppService transactionAppService, IRepository<FlagCard, long> flagCardRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _creditCardRepository = creditCardRepository;
            _transactionAppService = transactionAppService;
            _flagCardRepository = flagCardRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [HttpGet]
        [Route("list-flags")]
        public async Task<List<FlagCardDto>> GetAllListFlagsAsync()
        {
            List<FlagCard> allFlags = await _flagCardRepository.GetAllListAsync();
            return ObjectMapper.Map<List<FlagCardDto>>(allFlags);
        }

        [HttpGet]
        public async Task<List<CreditCardDto>> GetAllListAsync()
        {
            List<CreditCard> allCards = await _creditCardRepository.GetAll()
                .Include(x => x.Transactions)
                    .ThenInclude(x => x.Categories)
                .Include(x => x.Account)
                    .ThenInclude(x => x.Bank)
                .Include(x => x.FlagCard)
                .ToListAsync();

            return ObjectMapper.Map<List<CreditCardDto>>(allCards);
        }

        [HttpPost]
        public async Task CreateAsync(CreateCreditCardDto input)
        {
            CreditCard card = ObjectMapper.Map<CreditCard>(input);
            card.IsActive = true;
            card.Origin = Accounts.AccountConsts.AccountOrigin.Manual;
            await _creditCardRepository.InsertAsync(card);
        }

        [HttpPut]
        public async Task StatusChangeCardAsync(long id, bool statusChange)
        {
            var card = await _creditCardRepository.FirstOrDefaultAsync(id);
            card.IsActive = statusChange;

            await _creditCardRepository.UpdateAsync(card);
        }

        [HttpPost]
        [Route("PluggyCreateCreditCard")]
        public async Task PluggyCreateCreditCard(PluggyAccount input, long accountId)
        {
            var creditCardAlreadysaved = await GetAllListAsync();
            var isItemAlreadySaved = creditCardAlreadysaved.Any(x => x.PluggyItemId == input.ItemId && x.PluggyCreditCardId == input.Id);
            if (!isItemAlreadySaved)
            {
                var flagCreated = await CreateFlagFromPluggy(input.CreditData.Brand);
                var creditCardDto = new CreditCardDto
                {
                    CardName = input.Name,
                    CardNumber = "**** **** **** ****",
                    AccountId = accountId,
                    FlagCardId = flagCreated.Id,
                    Limit = input.CreditData.CreditLimit,
                    DueDay = input.CreditData.BalanceDueDate.Day,
                    IsActive = true,
                    Origin = AccountConsts.AccountOrigin.Pluggy,
                    PluggyItemId = input.ItemId,
                    PluggyCreditCardId = input.Id,
                };

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await _creditCardRepository.InsertAsync(ObjectMapper.Map<CreditCard>(creditCardDto));
                    uow.Complete();
                }
            }
        }

        [HttpDelete]
        public async Task DeleteAsync(long creditCardId)
        {
            var creditCard = await _creditCardRepository.FirstOrDefaultAsync(creditCardId);

            // Remova manualmente as transações associadas ao cartão
            foreach (var transaction in creditCard.Transactions.ToList())
            {
                await _transactionAppService.DeleteAsync(transaction.Id);
            }

            // Continue com a exclusão do cartão
            await _creditCardRepository.DeleteAsync(creditCard);
        }
        private async Task<(long Id, string FlagName)> CreateFlagFromPluggy(string input)
        {
            var allFlagsProsperium = await _flagCardRepository.GetAllListAsync();
            var isItemAlreadySaved = allFlagsProsperium.Where(x => x.Name.ToLower().Trim() == input.ToLower().Trim()).FirstOrDefault();
            if (isItemAlreadySaved == null)
            {
                FlagCard newFlag = new FlagCard
                {
                    Name = input
                };

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    await _flagCardRepository.InsertAndGetIdAsync(newFlag);
                    uow.Complete();
                    return (newFlag.Id, newFlag.Name);
                }
            }

            return (isItemAlreadySaved.Id, isItemAlreadySaved.Name);
        }
    }
}
