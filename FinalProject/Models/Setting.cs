using FinalProject.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Key { get; set; }
        [MaxLength(800)]
        public string? Value { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
    }
}
