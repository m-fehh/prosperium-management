using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.Banks
{
    [Table("Pxp_Banks")]
    public class Bank : Entity<long>
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
