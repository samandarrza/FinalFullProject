using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string Title1 { get; set; }
        [MaxLength(40)]
        public string Title2 { get; set; }
        [MaxLength(80)]
        public string Image { get; set; }
        [MaxLength(40)]
        public string BtnText { get; set; }
        [MaxLength(80)]
        public string RedirectUrl { get; set; }
        public int Order { get; set; }
    }
}
