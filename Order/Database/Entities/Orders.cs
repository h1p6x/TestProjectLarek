using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Order.Models;

namespace Order.Database.Entities
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? ClientName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        [Required]
        public bool Delivery { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
    }
}
