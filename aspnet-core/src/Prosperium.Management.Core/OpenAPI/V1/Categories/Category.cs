using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    [Table("P_Categories")]
    public class Category : Entity<long>
    {
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; } 

        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
