using Order.Models;

namespace Order.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderItemModel>> GetOrderItems(long orderId);
        public Task<OrderModel> GetOrderById(long orderId);
        public Task<OrderItemModel> AddToOrder(long productId, long orderId, int count);
        public Task<OrderModel> CreateOrder(ClientModel clientInfo);
        public Task<OrderModel> ConfirmOrder(long orderId);
        public Task<OrderModel> CancelOrder(long orderId);
        public Task<OrderModel> FinishOrder(long orderId);
        public Task<IEnumerable<ItemModelWithCount>> GetProducts(long orderId);
    }
}
