using Abp.Domain.Entities;
using Prosperium.Management.OpenAPI.V1.Categories;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Subcategories
{
    [Table("P_Subcategories")]
    public class Subcategory : Entity<long>
    {
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
