using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Flags
{
    [Table("Pxp_Flags")]
    public class FlagCard : Entity<long>
    {
        public string IconPath { get; set; }
        public string Name { get; set; }
    }
}
