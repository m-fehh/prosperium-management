using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;
using Prosperium.Management.MultiTenancy;

namespace Prosperium.Management.EntityFrameworkCore
{
    public class ManagementDbContext : AbpZeroDbContext<Tenant, Role, User, ManagementDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
            : base(options)
        {
        }
    }
}
