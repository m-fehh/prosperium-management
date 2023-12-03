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
using Prosperium.Management.OpenAPI.V1.Flags;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Tags;
using Prosperium.Management.OriginDestinations;

namespace Prosperium.Management.EntityFrameworkCore
{
    public class ManagementDbContext : AbpZeroDbContext<Tenant, Role, User, ManagementDbContext>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<AccountFinancial> Accounts { get; set; }
        public DbSet<FlagCard> Flags { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<OriginDestination> OriginDestinations { get; set; }

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

            // Relacionamentos

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CreditCard)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CreditCardId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Categories)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountFinancial>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountFinancial>()
                .HasMany(a => a.CreditCards)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CreditCard>()
                .HasOne(c => c.Account)
                .WithMany(a => a.CreditCards)
                .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<AccountFinancial>()
                .HasOne(a => a.Bank)
                .WithMany()
                .HasForeignKey(a => a.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreditCard>()
                .HasOne(c => c.FlagCard)
                .WithMany()
                .HasForeignKey(c => c.FlagCardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
