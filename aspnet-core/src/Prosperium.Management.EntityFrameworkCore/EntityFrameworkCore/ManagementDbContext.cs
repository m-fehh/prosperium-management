using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Prosperium.Management.Authorization.Roles;
using Prosperium.Management.Authorization.Users;
using Prosperium.Management.MultiTenancy;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Flags;
using Prosperium.Management.OpenAPI.V1.CreditCards;
using Prosperium.Management.OpenAPI.V1.Tags;
using Prosperium.Management.OriginDestinations;
using Prosperium.Management.OpenAPI.V1.Customers;
using Prosperium.Management.OpenAPI.V1.Banks;

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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerPhones> CustomerPhones { get; set; }
        public DbSet<CustomerEmails> CustomerEmails { get; set; }
        public DbSet<CustomerAddresses> CustomerAddresses { get; set; }

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

            modelBuilder.Entity<CustomerPhones>()
                .Property(p => p.Type)
                .HasConversion<string>();

            modelBuilder.Entity<CustomerEmails>()
                .Property(e => e.Type)
                .HasConversion<string>();

            modelBuilder.Entity<CustomerAddresses>()
                .Property(a => a.Type)
                .HasConversion<string>();     
            
            modelBuilder.Entity<Customer>()
                .Property(a => a.Origin)
                .HasConversion<string>();

            // Relacionamentos

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CreditCard)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CreditCardId)
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<CreditCard>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.CreditCard)
                .HasForeignKey(t => t.CreditCardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountFinancial>()
                .HasMany(a => a.CreditCards)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CreditCard>()
                .HasOne(c => c.Account)
                .WithMany(a => a.CreditCards)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

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


            // Configuração do relacionamento Customer -> CustomerPhones
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Phones)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento Customer -> CustomerEmails
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Emails)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento Customer -> CustomerAddresses
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
