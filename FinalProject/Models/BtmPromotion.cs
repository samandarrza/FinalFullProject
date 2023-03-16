using FinalProject.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class BtmPromotion
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Title { get; set; }
        [MaxLength(80)]
        public string? Image { get; set; }
        [MaxLength(40)]
        public string BtnText { get; set; }
        [MaxLength(80)]
        public string RedirectUrl { get; set; }
        [NotMapped]
        [MaxFileSize(2048)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile? ImageFile { get; set; }
    }
}
