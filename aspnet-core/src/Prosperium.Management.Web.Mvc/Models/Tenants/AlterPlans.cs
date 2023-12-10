using Prosperium.Management.MultiTenancy.Dto;
using Prosperium.Management.Plans;
using System.Collections.Generic;

namespace Prosperium.Management.Web.Models.Tenants
{
    public class AlterPlans
    {
        public List<Plan> Plans { get; set; }
        public TenantDto Tenant { get; set; }
    }
}
