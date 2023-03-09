using FinalProject.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
	public class MemberUpdateVM
	{
        [MaxLength(25)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
        [MaxLength(25)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password don't match")]
        public string? ConfirmPassword { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
    }
}
