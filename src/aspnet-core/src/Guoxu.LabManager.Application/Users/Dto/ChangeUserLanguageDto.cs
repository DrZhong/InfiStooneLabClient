using System.ComponentModel.DataAnnotations;

namespace Guoxu.LabManager.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}