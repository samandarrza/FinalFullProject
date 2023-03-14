using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Contact:BaseEntity
    {
        [MaxLength(30)]
        public string FullName { get; set; }
        [MaxLength(30)]
        public string Email { get; set; }
        [MaxLength(250)]
        public string Note { get; set; }
    }
}
