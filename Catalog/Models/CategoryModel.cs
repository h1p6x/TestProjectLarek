using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.Models
{
    public class CategoryModel
    {
        [Key]
        [JsonIgnore]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public IList<BrandModel> Brands { get; set; }
        public IList<ItemModel> Items { get; set; }
    }

    public class CategoryCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}