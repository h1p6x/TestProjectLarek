using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Delivery.Models
{
	public class DeliveryItemModel
	{
		[Key]
		[JsonIgnore]
		public long Id { get; set; }
		[Required]
		public long OrderId { get; set; }
		[Required]
		public DateTime CreatedDate { get; set; }
	}
}
