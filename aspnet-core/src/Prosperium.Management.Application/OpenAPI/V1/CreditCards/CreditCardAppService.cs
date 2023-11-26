using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts.Dto;
using Prosperium.Management.OpenAPI.V1.CreditCards.Dto;
using Prosperium.Management.OpenAPI.V1.Flags;
using Prosperium.Management.OpenAPI.V1.Flags.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    [Route("v1/cards")]
    public class CreditCardAppService : ManagementAppServiceBase, ICreditCardAppService
    {
        private readonly IRepository<CreditCard, long> _creditCardRepository;
        private readonly IRepository<FlagCard, long> _flagCardRepository;

        public CreditCardAppService(IRepository<CreditCard, long> creditCardRepository, IRepository<FlagCard, long> flagCardRepository)
        {
            _creditCardRepository = creditCardRepository;
            _flagCardRepository = flagCardRepository;
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
            await _creditCardRepository.InsertAsync(card);
        }

        [HttpPut]
        public async Task StatusChangeCardAsync(long id, bool statusChange)
        {
            var card = await _creditCardRepository.FirstOrDefaultAsync(id);
            card.IsActive = statusChange;

            await _creditCardRepository.UpdateAsync(card);
        }
    }
}
