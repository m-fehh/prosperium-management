using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.OpenAPI.V1.Categories
{
    [Table("Pxp_Categories")]
    public class Category : Entity<long>
    {
        public string IconPath { get; set; }
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; } 

        public virtual ICollection<Subcategory> Subcategories { get; set; }
        public bool IsVisible { get; set; }
    }
}
