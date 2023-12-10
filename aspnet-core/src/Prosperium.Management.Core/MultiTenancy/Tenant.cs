using Abp.MultiTenancy;
using Prosperium.Management.Authorization.Users;
using System;

namespace Prosperium.Management.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name, int planId, string planName, DateTime? planExpiration)
            : base(tenancyName, name)
        {
            PlanId = planId;
            PlanName = planName;
            PlanExpiration = planExpiration;
        }

        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public DateTime? PlanExpiration { get; set; }
    }
}
