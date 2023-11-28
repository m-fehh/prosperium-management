using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.ExternalServices.Pluggy.Models
{
    [Table("Pxp_Pluggy_Categories")]
    public class PluggyCategory : Entity
    {
        public string Pluggy_Id { get; set; }
        public string Pluggy_Category_Id { get; set; }
        public string Pluggy_Category_Name { get; set; }
        public string Pluggy_Category_Name_Translated { get; set; }
        public string Pluggy_Description { get; set; }
        public string Pluggy_Description_Translated { get; set; }
        public long? Prosperium_Category_Id { get; set; }
    }
}
