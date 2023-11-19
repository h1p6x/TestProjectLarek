using Delivery.Database.Entities;
using Delivery.Models;

namespace Delivery.Services
{
	public interface IDeliveryService
	{
        public Task<OrderModel> GetOrderInfo(long orderId);
        public Task<IEnumerable<ItemModelWithCount>> GetProducts(long orderId);
        public Task<OrderModel> ReturnOrder(long orderId);
        public Task<OrderModel> FinishOrder(long orderId);
        public Task<DeliveryItemModel> RecordOrder(DeliveryItemModel item);
    }
}
