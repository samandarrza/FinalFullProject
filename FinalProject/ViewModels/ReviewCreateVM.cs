using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ReviewCreateVM
    {
        [Range(1, 5)]
        public byte Rate { get; set; }
        [MaxLength(50)]
        public string Text { get; set; }
        public int PhoneId { get; set; }
    }
}
