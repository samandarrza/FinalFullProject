using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProject.Enums;

namespace FinalProject.Models
{
    public class Order:BaseEntity
    {
        public string? AppUserId { get; set; }
        [MaxLength(25)]
        public string Fullname { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [MaxLength(200)]
        public string? Note { get; set; }

        public AppUser? AppUser { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}
