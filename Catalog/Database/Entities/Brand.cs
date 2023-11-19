using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Database.Entities
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // Внешний ключ для связи с сущностью Category
        [ForeignKey("CategoryId")]
        public long CategoryId { get; set; }
        public Category Category { get; set; } // Навигационное свойство к категории

        // Навигационное свойство для связи с товарами
        public ICollection<Item> Items { get; set; }
    }
}