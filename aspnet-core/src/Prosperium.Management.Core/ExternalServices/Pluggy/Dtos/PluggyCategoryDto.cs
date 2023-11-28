using Abp.Application.Services.Dto;

namespace Prosperium.Management.ExternalServices.Pluggy.Dtos
{
    public class CategoryPluggyDto : EntityDto
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
