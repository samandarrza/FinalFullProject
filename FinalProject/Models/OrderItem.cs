using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int OrderId { get; set; }
        public int? PhoneId { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        public Order Order { get; set; }
        public Phone? Phone { get; set; }
    }
}
