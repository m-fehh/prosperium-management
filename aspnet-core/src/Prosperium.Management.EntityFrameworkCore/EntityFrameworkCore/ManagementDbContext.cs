using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.Banks;
using Prosperium.Management.OpenAPI.V1.Accounts;

namespace Prosperium.Management.EntityFrameworkCore
{
    public class ManagementDbContext : AbpZeroDbContext<Tenant, Role, User, ManagementDbContext>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<AccountFinancial> Accounts { get; set; }

        public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversões

            modelBuilder.Entity<Transaction>()
                .Property(e => e.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(32);

            modelBuilder.Entity<Category>()
                .Property(e => e.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(32);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.PaymentType)
                .HasConversion<string>()
                .HasMaxLength(32);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.PaymentTerm)
                .HasConversion<string>()
                .HasMaxLength(32);
        }
    }
}
