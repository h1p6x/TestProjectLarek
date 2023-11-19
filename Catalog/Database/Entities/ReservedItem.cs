using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Database.Entities
{
    public class ReservedItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        // Внешний ключ для связи с сущностью Item
        [ForeignKey("ItemId")]
        public long ItemId { get; set; }
        public Item Item { get; set; } // Навигационное свойство к товару
        [Required]
        public int Count { get; set; }
    }
}
