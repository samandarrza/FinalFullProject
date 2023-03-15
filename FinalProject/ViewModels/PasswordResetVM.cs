using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class PasswordResetVM
    {
        public string Email { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
