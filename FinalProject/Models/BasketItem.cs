namespace FinalProject.Models
{
    public class BasketItem:BaseEntity
    {
        public string AppUserId { get; set; }
        public int PhoneId { get; set; }
        public int Count { get; set; }

        public AppUser AppUser { get; set; }
        public Phone Phone { get; set; }
    }
}
