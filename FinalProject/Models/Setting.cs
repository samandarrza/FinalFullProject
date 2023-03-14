using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string? Value { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
