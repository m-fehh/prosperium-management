using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static Prosperium.Management.OpenAPI.V1.Accounts.AccountConsts;

namespace Prosperium.Management.Banks
{
    [Table("Pxp_Banks")]
    public class Bank : Entity<long>
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public AccountOrigin Origin { get; set; } = AccountOrigin.Manual;
    }
}
