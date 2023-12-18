using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.OpenAPI.V1.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Prosperium.Management.Plans
{
    public class PlansManager : IDomainService
    {
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<AccountFinancial, long> _accountFinancialRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PlansManager(IRepository<Plan> planRepository, IRepository<AccountFinancial, long> accountFinancialRepository, IRepository<Tenant> tenantRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _planRepository = planRepository;
            _accountFinancialRepository = accountFinancialRepository;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        // Validates number of accounts created
        public async Task<bool> ValidatesCreatedAccounts(int tenantId)
        {
            Plan tenantPlan = await CapturePlanByTenantId(tenantId);
            int numberOfAccountsCreated = await _accountFinancialRepository.GetAll().Where(x => x.TenantId == tenantId).CountAsync();

            return numberOfAccountsCreated < tenantPlan.MaxAccounts; // Se retornar false, o inquilino atingiu o limite
        }

        public async Task<bool> ValidatesIntegrationPluggy(int tenantId)
        {
            return (await CapturePlanByTenantId(tenantId)).IntegrationPluggy;
        }

        public async Task ChangePlanOnExpiration()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                Plan planDefault;

                var expiredTenants = await _tenantRepository.GetAll()
                    .Where(t => t.PlanExpiration.HasValue && t.PlanExpiration < DateTime.Now)
                    .ToListAsync();

                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    planDefault = await _planRepository.GetAll().Where(x => x.Name.ToLower().Trim() == "essencial").FirstOrDefaultAsync();
                }

                foreach (var item in expiredTenants)
                {
                    await ChangeTenantPlan(item.Id, planDefault.Id, planDefault.Name, null);
                }

                await unitOfWork.CompleteAsync();
            }
        }


        public async Task ChangeTenantPlan(int tenantId, int selectedPlanId, string selectedPlanName, string formattedDate)
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(tenantId);
            tenant.PlanId = selectedPlanId;
            tenant.PlanName = selectedPlanName;
            tenant.PlanExpiration = (!string.IsNullOrEmpty(formattedDate)) ? Convert.ToDateTime(formattedDate) : null;

            await _tenantRepository.UpdateAsync(tenant);
        }

        private async Task<Plan> CapturePlanByTenantId(int tenantId)
        {
            var planId = await _tenantRepository.GetAll().Where(x => x.Id == tenantId).Select(x => x.PlanId).FirstOrDefaultAsync();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await _planRepository.FirstOrDefaultAsync(x => x.Id == planId);
            }
        }
    }
}
