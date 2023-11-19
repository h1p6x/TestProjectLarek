using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Database.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        // Внешний ключ для связи с сущностью Brand
        [ForeignKey("BrandId")]
        public long BrandId { get; set; }
        public Brand Brand { get; set; } // Навигационное свойство к бренду

        [Required]
        // Внешний ключ для связи с сущностью Category
        [ForeignKey("CategoryId")]
        public long CategoryId { get; set; }
        public Category Category { get; set; } // Навигационное свойство к категории

        public bool IsAvailable { get; set; }
    }
}
