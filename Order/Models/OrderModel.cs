using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Order.Models
{
    public class OrderModel
    {
        [Key]
        [JsonIgnore]
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
