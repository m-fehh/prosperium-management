using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using System.ComponentModel.DataAnnotations;
using Prosperium.Management.OpenAPI.V1.Flags;
using System.Collections.Generic;
using Prosperium.Management.OpenAPI.V1.Transactions;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    [Table("Pxp_Cards")]
    public class CreditCard : AuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public virtual long AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountFinancial Account { get; set; }
        public virtual long FlagCardId { get; set; }

        [ForeignKey("FlagCardId")]
        public FlagCard FlagCard { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Limit { get; set; }
        public int DueDay { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public bool IsActive { get; set; }
        public AccountOrigin Origin { get; set; }
        public string PluggyItemId { get; set; }
        public string PluggyCreditCardId { get; set; }
    }
}
