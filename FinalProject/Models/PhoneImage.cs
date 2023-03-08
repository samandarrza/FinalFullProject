using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class PhoneImage
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int PhoneId { get; set; }
        public bool? Status { get; set; }
        public Phone Phone { get; set; }
    }
}
