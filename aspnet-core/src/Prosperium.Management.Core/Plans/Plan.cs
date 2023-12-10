using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.Plans
{
    [Table("Pxp_Plans")]
    public class Plan : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int MaxAccounts { get; set; }
        public bool IntegrationPluggy {  get; set; }
        public bool Transfer { get; set; }
    }
}
