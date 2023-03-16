namespace FinalProject.Models
{
    public class wishlistItem:BaseEntity
    {
        public string AppUserId { get; set; }
        public int PhoneId { get; set; }

        public AppUser AppUser { get; set; }
        public Phone Phone { get; set; }
    }
}
