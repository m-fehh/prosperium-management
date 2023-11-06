using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}