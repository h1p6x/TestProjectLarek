using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.Models
{
    public class BrandModel
    {
        [Key]
        [JsonIgnore]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
    public class BrandCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Category { get; set; }
    }
}