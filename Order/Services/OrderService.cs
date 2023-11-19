using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Order.Database;
using Order.Database.Entities;
using Order.Models;

namespace Order.Services
{
    public class OrderService : IOrderService
	{
		private readonly OrderDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly IHttpClientFactory _httpFactory;
		private readonly HttpContext _httpContext;

		public OrderService(OrderDbContext dbContext, IMapper mapper, IHttpClientFactory httpFactory, 
																	  IHttpContextAccessor httpContext)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_httpFactory = httpFactory;
			_httpContext = httpContext.HttpContext;
		}

		public async Task<IEnumerable<OrderItemModel>> GetOrderItems(long orderId)
		{
			var orderItems = await _dbContext.OrderItems
				.Where(oi => oi.OrderId == orderId)
				.ToListAsync();

			return _mapper.Map<IEnumerable<OrderItemModel>>(orderItems);
		}


		public async Task<OrderModel> GetOrderById(long orderId)
		{
			var order = await _dbContext.Orders
				.SingleOrDefaultAsync(o => o.Id == orderId);
			if (order is null)
				throw new ArgumentException($"Заказ с id = {orderId} не найден");
			
			return _mapper.Map<OrderModel>(order);
		}
		
		public async Task<OrderModel> CreateOrder(ClientModel clientInfo)
		{
			var newOrder = new Orders
			{
				ClientName = clientInfo.Name,
				Phone = clientInfo.Phone,
				Address = clientInfo.Address,
				Delivery = clientInfo.Delivery,
				OrderStatus = OrderStatus.Pending,
				CreatedTime = DateTime.UtcNow
			};

			_dbContext.Orders.Add(newOrder);
			await _dbContext.SaveChangesAsync();

			return _mapper.Map<OrderModel>(newOrder);
		}
		
		public async Task<OrderItemModel> AddToOrder(long productId, long orderId, int count)
		{
			if (count <= 0)
				throw new ArgumentException("Количество должно быть больше нуля");
			
			var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

			if (order == null || order.OrderStatus == OrderStatus.Finished || order.OrderStatus == OrderStatus.Canceled)
				throw new ArgumentException("Невозможно добавить товар для доставленного или отмененного заказа");

			// Проверка наличия товара и правильности количества
			var isProductValid = await IsProductExists(productId, count);

			if (!isProductValid)
				throw new ArgumentException($"Такого товара нет, либо неправильно указано количество");

			var orderItem = await _dbContext.OrderItems
				.SingleOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);

			if (orderItem == null)
			{
				// Создание нового элемента заказа и добавление его к заказу
				orderItem = new OrderItem { OrderId = orderId, ProductId = productId, Count = count };
				_dbContext.OrderItems.Add(orderItem);
			}
			else
			{
				// Обновление количества товара в элементе заказа
				orderItem.Count += count;
				var entry = _dbContext.Entry(orderItem);
				entry.Property(x => x.Count).IsModified = true;
			}

			await _dbContext.SaveChangesAsync();
			return _mapper.Map<OrderItemModel>(orderItem);
		}

		
		public async Task<OrderModel> ConfirmOrder(long orderId)
		{
			var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

			if (existingOrder == null && existingOrder.OrderStatus != OrderStatus.Undefined || 
														existingOrder.OrderStatus != OrderStatus.Pending)
				throw new ArgumentException("Невозможно подтвердить данный заказ");
			
			var orderItems = await GetOrderItems(orderId);

			if (orderItems == null || !orderItems.Any())
				throw new ArgumentException("В заказе нет товаров");
			
			var catalogItems = orderItems.Select(item => new
			{
				itemId = item.ProductId,
				count = item.Count
			}).ToList();
			var catalogRequest = "api/Catalog/Reserve/Add";
			var catalogService = _httpFactory.CreateClient("CatalogService");
			await catalogService.PostAsJsonAsync(catalogRequest, catalogItems);
			
			var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order != null)
			{
				order.OrderStatus = OrderStatus.Created;
				var entry = _dbContext.Entry(order);
				entry.Property(x => x.OrderStatus).IsModified = true;

				await _dbContext.SaveChangesAsync();
			}
			
            return _mapper.Map<OrderModel>(order);
		}

		public async Task<OrderModel> CancelOrder(long orderId)
		{
			var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			
			if (existingOrder == null)
				throw new ArgumentException("Такого заказа не существует");

			if (existingOrder != null && existingOrder.OrderStatus != OrderStatus.Created)
				throw new ArgumentException($"Статус заказа должен быть \"Создан\"");

			var catalogRequest = "api/Catalog/CancelReserve";
			var items = await GetOrderItems(orderId);
			
			var reservedItems = items.Select(item => new
			{
				itemId = item.ProductId,
				count = item.Count
			}).ToList();

			var catalogService = _httpFactory.CreateClient("CatalogService");
			await catalogService.PostAsJsonAsync(catalogRequest, reservedItems);
			
			var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order != null)
			{
				order.OrderStatus = OrderStatus.Canceled;
				var entry = _dbContext.Entry(order);
				entry.Property(x => x.OrderStatus).IsModified = true;

				await _dbContext.SaveChangesAsync();
			}

			return _mapper.Map<OrderModel>(order);
		}

        public async Task<OrderModel> FinishOrder(long orderId)
        {
            var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (existingOrder == null)
                throw new ArgumentException("Такого заказа не существует");

            if (existingOrder != null && existingOrder.OrderStatus != OrderStatus.Created)
                throw new ArgumentException($"Статус заказа должен быть \"Создан\"");

            var catalogRequest = "api/Catalog/RemoveReserve";
            var items = await GetOrderItems(orderId);

            var reservedItems = items.Select(item => new
            {
                itemId = item.ProductId,
                count = item.Count
            }).ToList();

            var catalogService = _httpFactory.CreateClient("CatalogService");
            await catalogService.PostAsJsonAsync(catalogRequest, reservedItems);

            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Finished;
                var entry = _dbContext.Entry(order);
                entry.Property(x => x.OrderStatus).IsModified = true;

                await _dbContext.SaveChangesAsync();
            }

            return _mapper.Map<OrderModel>(order);
        }


        private async Task<bool> IsProductExists(long id, int count)
		{
			var requestUri = $"api/Catalog/ItemExists/{id}?itemCount={count}";
			var serviceName = "CatalogService";
			var client = _httpFactory.CreateClient(serviceName);

			var response = await client.GetAsync(requestUri);

			if (response.IsSuccessStatusCode)
			{
				var isExists = await response.Content.ReadFromJsonAsync<bool>();
				return isExists;
			}

			return false;
		}

        public async Task<IEnumerable<ItemModelWithCount>> GetProducts(long orderId)
        {
	        // Получение информации о товарах
	        var items = await GetOrderItems(orderId);
	        var itemsIds = items.Select(x => x.ProductId).ToList();
	        
	        var requestUri = $"api/Catalog/GetItemsByIds?ids={string.Join("&ids=", itemsIds)}";

	        var catalogService = _httpFactory.CreateClient("CatalogService");
	        var response = await catalogService.GetAsync(requestUri);

	        if (response.IsSuccessStatusCode)
	        {
		        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ItemModel>>();
		        if (products != null)
		        {
			        var productsWithCount = products.Select(product =>
			        {
				        var itemInOrder = items.FirstOrDefault(item => item.ProductId == product.Id);
				        int countInOrder = itemInOrder?.Count ?? 0;

				        return new ItemModelWithCount
				        {
					        Item = product,
					        CountInOrder = countInOrder
				        };
			        });

			        return productsWithCount;
		        }
	        }

	        return Enumerable.Empty<ItemModelWithCount>();
        }
    }
}
