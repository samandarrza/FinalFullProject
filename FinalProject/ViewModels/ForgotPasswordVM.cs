using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ForgotPasswordVM
    {
        [MaxLength(80)]
        public string Email { get; set; }
    }
}
