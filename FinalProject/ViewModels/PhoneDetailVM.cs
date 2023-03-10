using FinalProject.Areas.admin.ViewModels;
using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class PhoneDetailVM
    {
        public Phone Phone { get; set; }
        public List<Phone> RelatedPhone { get; set; }
        public ReviewCreateVM ReviewVM { get; set; }
        public bool HasReview { get; set; }

    }
}
