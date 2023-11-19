using AutoMapper;
using Delivery.Database;
using Delivery.Database.Entities;
using Delivery.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Delivery.Services
{
    public class DeliveryService : IDeliveryService
	{
		private readonly DeliveryDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly IHttpClientFactory _httpFactory;
		private readonly HttpContext _httpContext;

		public DeliveryService(DeliveryDbContext dbContext, IMapper mapper, IHttpClientFactory httpFactory,
																	  IHttpContextAccessor httpContext)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_httpFactory = httpFactory;
			_httpContext = httpContext.HttpContext;
		}

        public async Task<OrderModel> GetOrderInfo(long orderId)
        {
            var orderRequest = $"api/Order/GetOrderById/{orderId}";
            var orderService = _httpFactory.CreateClient("OrderService");
            var orderInfo = await orderService.GetFromJsonAsync<OrderModel>(orderRequest);
            
			return _mapper.Map<OrderModel>(orderInfo);
        }

		public async Task<IEnumerable<ItemModelWithCount>> GetProducts(long orderId)
		{
			var orderRequest = $"api/Order/GetProductsByOrdeId/{orderId}";
			var orderService = _httpFactory.CreateClient("OrderService");
			var products = await orderService.GetFromJsonAsync<IEnumerable<ItemModelWithCount>>(orderRequest);

			return _mapper.Map<IEnumerable<ItemModelWithCount>>(products);
		}

        public async Task<OrderModel> ReturnOrder(long orderId)
        {
            var order = await _dbContext.DeliveryItems.FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order is null)
                throw new ArgumentException("Такого заказа не существует в доставке");

            var catalogRequest = $"api/Order/CancelOrder/{orderId}";
            var catalogService = _httpFactory.CreateClient("OrderService");
            var response = await catalogService.PostAsync(catalogRequest, null);

            if (response.IsSuccessStatusCode)
            {

                // Успешный запрос, извлекаем содержимое и десериализируем
                var content = await response.Content.ReadAsStringAsync();
                var orderModel = JsonConvert.DeserializeObject<OrderModel>(content);
                _dbContext.DeliveryItems.Remove(order);
                await _dbContext.SaveChangesAsync();

                return orderModel;
            }
            else
                throw new Exception($"Error from OrderService: {response.StatusCode}");
        }

        public async Task<OrderModel> FinishOrder(long orderId)
        {
            var order = await _dbContext.DeliveryItems.FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order is null)
                throw new ArgumentException("Такого заказа не существует в доставке");
            
            var orderRequest = $"api/Order/FinishOrder/{orderId}";
            var orderService = _httpFactory.CreateClient("OrderService");
            var response = await orderService.PostAsync(orderRequest, null);

            if (response.IsSuccessStatusCode)
            {
                // Успешный запрос, извлекаем содержимое и десериализируем
                var content = await response.Content.ReadAsStringAsync();
                var orderModel = JsonConvert.DeserializeObject<OrderModel>(content);
                _dbContext.DeliveryItems.Remove(order);
                await _dbContext.SaveChangesAsync();

                return orderModel;
            }
            else
                throw new Exception($"Error from OrderService: {response.StatusCode}");
        }



        public async Task<DeliveryItemModel> RecordOrder(DeliveryItemModel item)
        {
            if (item == null)
                throw new ArgumentException("Модель пуста");
            
            // Проверка существования заказа
            var existingOrder = await GetOrderInfo(item.OrderId);

            if (existingOrder is null)
                throw new Exception($"Заказ с id = {item.OrderId} не найден");
            
            if (existingOrder.OrderStatus == OrderStatus.Finished)
                throw new ArgumentException($"Заказ с ID = {item.OrderId} уже завершен");

            if (existingOrder.OrderStatus == OrderStatus.Canceled)
                throw new ArgumentException($"Заказ с ID = {item.OrderId} уже отменен");
            
            var existingDeliveryItem = await _dbContext.DeliveryItems
                .FirstOrDefaultAsync(di => di.OrderId == item.OrderId);

            if (existingDeliveryItem != null)
                throw new ArgumentException($"Заказ с id = {item.OrderId} уже находится в доставке");
            
            var deliveryItem = new DeliveryItems
            {
                CreatedDate = item.CreatedDate,
                OrderId = item.OrderId
            };

            await _dbContext.DeliveryItems.AddAsync(deliveryItem);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<DeliveryItemModel>(deliveryItem);
        }
    }
}
