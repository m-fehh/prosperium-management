using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Prosperium.Management.OpenAPI.V1.Accounts;
using Prosperium.Management.OpenAPI.V1.Categories;
using System.ComponentModel.DataAnnotations;
using Prosperium.Management.OpenAPI.V1.Flags;
using System.Collections.Generic;
using Prosperium.Management.OpenAPI.V1.Transactions;

namespace Prosperium.Management.OpenAPI.V1.CreditCards
{
    [Table("Pxp_Cards")]
    public class CreditCard : AuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CardName { get; set; }
        public virtual long AccountId { get; set; }

        [ForeignKey("AccountId")]
        public AccountFinancial Account { get; set; }
        public virtual long FlagCardId { get; set; }

        [ForeignKey("FlagCardId")]
        public FlagCard FlagCard { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Limit { get; set; }

        [Range(1, 31, ErrorMessage = "O dia de vencimento deve estar entre 1 e 31.")]
        public int DueDayInput { get; set; }

        // Propriedade de string para exibição formatada
        public string DueDay => DueDayInput.ToString("D2");
        public ICollection<Transaction> Transactions { get; set; }
    }
}
