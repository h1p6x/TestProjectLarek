using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Database.Entities
{
    public class DeliveryItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public long OrderId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
