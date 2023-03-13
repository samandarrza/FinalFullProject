using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class AccountVM
    {
        public MemberUpdateVM MemberUpdateVM { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}
