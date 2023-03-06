using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class PhoneTag
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
