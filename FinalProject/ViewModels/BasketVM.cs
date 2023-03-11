namespace FinalProject.ViewModels
{
    public class BasketVM
    {
        public decimal TotalPrice { get; set; }
        public List<BasketItemVM> Items { get; set; } = new List<BasketItemVM>();
    }
}
