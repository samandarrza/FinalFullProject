using FinalProject.Models;

namespace FinalProject.Areas.admin.ViewModels
{
    public class DashboardVM
    {
        public int[]? SalesMovement { get; set; }
        public int[]? OrderStatus { get; set; }
        public List<Order> Orders { get; set; }

    }
}
