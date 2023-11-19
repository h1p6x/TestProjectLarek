using System.ComponentModel.DataAnnotations;

namespace Order.Models
{
    public class ClientModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool Delivery { get; set; }
    }
}
