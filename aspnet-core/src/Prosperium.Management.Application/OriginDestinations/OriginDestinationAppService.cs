using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Flurl;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Prosperium.Management.ExternalServices.Pluggy;
using Prosperium.Management.OpenAPI.V1.Categories.Dto;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Prosperium.Management.OriginDestinations
{
    public class OriginDestinationAppService : ManagementAppServiceBase, IOriginDestinationAppService
    {
        private readonly PluggyManager _pluggyManager;
        private readonly IRepository<OriginDestination> _originDestinationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OriginDestinationAppService(PluggyManager pluggyManager, IRepository<OriginDestination> originDestinationRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _pluggyManager = pluggyManager;
            _originDestinationRepository = originDestinationRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<PagedResultDto<GetOriginDestinationForViewDto>> GetAllOriginDestinationAsync(GetAllOriginDestinationFilter input)
        {
            var originDestination = _originDestinationRepository.GetAll()
                .Where(x => !string.IsNullOrEmpty(x.OriginValueName) && x.DestinationValueId == null);

            var pagedAndFiltered = originDestination.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var originDestinations = from o in pagedAndFiltered
                                     select new GetOriginDestinationForViewDto()
                                     {
                                         OriginDestinations = ObjectMapper.Map<OriginDestination>(o),
                                     };

            int totalCount = await originDestination.CountAsync();
            return new PagedResultDto<GetOriginDestinationForViewDto>
            (
                totalCount,
                await originDestinations.ToListAsync()
            );
        }

        public async Task UpdateDestinationValueAsync(int pluggyId, long prosperiumId, bool changeAllNames)
        {
            var originDestination = await _originDestinationRepository.GetAllListAsync();

            if (originDestination != null)
            {
                var searchById = originDestination.Where(x => x.Id == pluggyId).FirstOrDefault();
                if (changeAllNames)
                {
                    var allOriginDestinationByName = searchById.OriginValueName.Split('-')[0];
                    var destinationToUpdate = originDestination.Where(c => c.OriginValueName.Contains(allOriginDestinationByName)).ToList();
                    foreach (var item in destinationToUpdate)
                    {
                        item.DestinationValueId = prosperiumId.ToString();
                        await _originDestinationRepository.UpdateAsync(item);
                    }
                }
                else
                {
                    searchById.DestinationValueId = prosperiumId.ToString();
                    await _originDestinationRepository.UpdateAsync(searchById);
                }
            }
        }

        public async Task CaptureCategoriesByPluggy()
        {
            CultureInfo culturaDestino = new CultureInfo("pt");

            List<OriginDestination> OriginDestinationsAlreadySaved = await _originDestinationRepository.GetAllListAsync();
            List<OriginDestination> originDestination = new List<OriginDestination>();


            var allCategories = await _pluggyManager.PluggyGetCategories();
            if (allCategories.Total > 0)
            {
                foreach (var item in allCategories.Results)
                {
                    var isItemAlreadySaved = OriginDestinationsAlreadySaved.Any(x => x.OriginValueId == item.Id);
                    if (!isItemAlreadySaved)
                    {
                        originDestination.Add(new OriginDestination
                        {
                            OriginPortal = "Pluggy",
                            OriginValueId = item.Id,
                            OriginValueName = (string.IsNullOrEmpty(item.ParentDescriptionTranslated)) ? item.DescriptionTranslated : $"{item.ParentDescriptionTranslated} - {item.DescriptionTranslated}",
                            Discriminator = "Categoria",
                            DestinationPortal = "Prosperium",
                            DestinationValueId = null,
                            DestinationValueName = null
                        });
                    }
                }
            }

            await _originDestinationRepository.InsertRangeAsync(originDestination);
        }

        //public async Task CaptureBanksByPluggy(string itemId)
        //{
        //    List<OriginDestination> OriginDestinationsAlreadySaved = await _originDestinationRepository.GetAllListAsync();

        //    var connector = await _pluggyManager.PluggyGetItemId(itemId);
        //    if (connector != null)
        //    {
        //        var isItemAlreadySaved = OriginDestinationsAlreadySaved.Any(x => x.OriginValueId == connector.Id);
        //        if (!isItemAlreadySaved)
        //        {
        //            OriginDestination originDestination = new OriginDestination
        //            {
        //                OriginPortal = "Pluggy",
        //                OriginValueId = connector.Connector.Id,
        //                OriginValueName = connector.Connector.Name,
        //                Discriminator = "Bancos",
        //                DestinationPortal = "Prosperium",
        //                DestinationValueId = null,
        //                DestinationValueName = null
        //            };

        //            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        //            {
        //                await _originDestinationRepository.InsertAsync(originDestination);
        //                uow.Complete();
        //            }
        //        }
        //    }
        //}

        //public async Task CheckIfBankIsAlreadyMapped(string itemId)
        //{
        //    OriginDestination originDestinationsAlreadySaved = await _originDestinationRepository
        //        .GetAll()
        //        .Where(x => x.OriginValueId == itemId)
        //        .FirstOrDefaultAsync();

        //    if (originDestinationsAlreadySaved == null)
        //    {
        //        await CaptureBanksByPluggy(itemId);
        //    }
        //}

    }
}
