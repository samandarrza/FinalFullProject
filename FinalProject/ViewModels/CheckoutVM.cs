using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class CheckoutVM
    {
        public Order Order { get; set; }
        public List<CheckoutItemVM> CheckoutItems { get; set; } = new List<CheckoutItemVM>();
        public decimal Total { get; set; }
    }
}
