using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(30)]
        public string? FullName { get; set; }
        public string? Image { get; set; }
    }
}
