using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OriginDestinations
{
    [Table("Pxp_OriginDestination")]
    public class OriginDestination : Entity
    {
        public string OriginPortal { get; set; }
        public string OriginValueId { get; set; }
        public string OriginValueName{ get; set; }       
        public string Discriminator { get; set; }
        public string DestinationPortal { get; set; }
        public string DestinationValueId { get; set; }
        public string DestinationValueName { get; set; }

    }
}
