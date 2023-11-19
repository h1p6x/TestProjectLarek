using System.ComponentModel.DataAnnotations;

namespace Delivery.Models
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
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
    }

    public class ItemModelWithCount
    {
        public ItemModel Item { get; set; }
        public int CountInOrder { get; set; }
    }
}
