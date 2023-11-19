using System.ComponentModel.DataAnnotations;

namespace Catalog.Models
{
    public class ItemModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Article { get; set; }
        [Required] 
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;

    }

    public class ItemCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Article { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
