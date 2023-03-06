using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class PhoneModel
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
