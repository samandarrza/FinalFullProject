using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Review:BaseEntity
    {
        public int PhoneId { get; set; }
        public string AppUserId { get; set; }
        public byte Rate { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }

        public Phone phone { get; set; }
        public AppUser AppUser { get; set; }
    }
}
