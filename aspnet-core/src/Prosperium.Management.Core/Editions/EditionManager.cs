using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Prosperium.Management.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Basic, Advanced, Corporate";

        public EditionManager(
            IRepository<Edition> editionRepository,
            IAbpZeroFeatureValueStore featureValueStore,
            IUnitOfWorkManager unitOfWorkManager) 
            : base(editionRepository, featureValueStore, unitOfWorkManager)
        {
        }
    }
}
