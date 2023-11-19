using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Database.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // Навигационное свойство для связи с товарами
        public ICollection<Item> Items { get; set; }
        [InverseProperty("Category")]
        public ICollection<Brand> Brands { get; set; }
    }
}
