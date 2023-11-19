using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Order.Models
{
    public class OrderItemModel
    {
        [Key]
        [JsonIgnore]
        public long Id { get; set; }
        [Required]
        public long OrderId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
