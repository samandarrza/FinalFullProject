using FinalProject.Areas.admin.ViewModels;
using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class ShopVM
    {
        public PaginatedList<Phone>? Phones { get; set; }
        public List<Battery>? Batteries { get; set; }
        public List<Color>? Colors { get; set; }
        public List<Display>? Displays { get; set; }
        public List<Memory>? Memories { get; set; }
        public List<PhoneModel>? PhoneModels { get; set; }
        public List<PhoneSystem>? PhoneSystems { get; set; }
        public List<ProcessorName>? ProcessorNames { get; set; }
        public List<RAM>? RAMs { get; set; }

        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
