using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Banks;
using System.Linq;

namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class DefaultPlansCreator
    {
        private readonly ManagementDbContext _context;

        public DefaultPlansCreator(ManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            SeedPlans();
        }

        private void SeedPlans()
        {
            AddPlan("Essencial", 0, 1, false, false);
            AddPlan("Avançado", 69.90M, 10, true, true);

            SaveChanges();
        }

        private void AddPlan(string name, decimal price, int maxAccount, bool transfer, bool integrationPluggy)
        {
            var plan = _context.Plans.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name);
            if (plan == null)
            {
                plan = new Plans.Plan
                {
                    Name = name,
                    Price = price,
                    MaxAccounts = maxAccount,
                    Transfer = transfer,
                    IntegrationPluggy = integrationPluggy
                };

                _context.Plans.Add(plan);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
