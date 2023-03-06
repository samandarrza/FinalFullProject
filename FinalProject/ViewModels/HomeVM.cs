using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<BtmPromotion> BtmPromotions { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Phone> MostSoldPhones { get; set; }
        public List<Phone> IsNewPhones { get; set; }
        public List<Phone> DiscountedPhones { get; set; }

    }
}
