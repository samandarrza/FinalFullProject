using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.ViewModels
{
    public class WishlistVM
    {
        public List<WishlistItemVM> Items { get; set; } = new List<WishlistItemVM>();
        [Column(TypeName = "decimal(18,2)")]
        public decimal totalPrice { get; set; }
    }
}
