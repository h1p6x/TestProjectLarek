using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.Models
{
    public class ReservedItemModel
    {
        [Key]
        [JsonIgnore]
        public long Id { get; set; }
        [Required]
        public long ItemId { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
