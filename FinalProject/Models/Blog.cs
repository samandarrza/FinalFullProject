using FinalProject.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Blog:BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(80)]
        public string? Image { get; set; }
        [MaxLength(30)]
        public string Tag { get; set; }
        [MaxLength(800)]
        public string Description { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
    }
}
