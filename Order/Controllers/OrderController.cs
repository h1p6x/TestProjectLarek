using Microsoft.AspNetCore.Mvc;
using Order.Models;
using Order.Services;

namespace Order.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IOrderService _service;
		public OrderController(ILogger<OrderController> logger, IOrderService service)
		{
			_logger = logger;
			_service = service;
		}

        /// <summary>
        /// Получение заказа по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderById(long id)
        {
            try
            {
                var res = await _service.GetOrderById(id);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Получение товаров в определенном заказе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(OrderItemModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderItems(long id)
        {
            try
            {
                var res = await _service.GetOrderItems(id);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        
        /// <summary>
        /// Создание заказа
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(OrderItemModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(ClientModel clientInfo)
        {
            try
            {
                var res = await _service.CreateOrder(clientInfo);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Добавить в заказ
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="orderId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(OrderItemModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToOrder(long productId, long orderId, int count)
        {
            try
            {
                var res = await _service.AddToOrder(productId, orderId, count);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Подтвердить заказ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        [HttpPost("[action]/{orderId}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmOrder(long orderId)
        {
            try
            {
                var res = await _service.ConfirmOrder(orderId);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("[action]/{orderId}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(long orderId)
        {
            try
            {
                var res = await _service.CancelOrder(orderId);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Завершить заказ
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("[action]/{orderId}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FinishOrder(long orderId)
        {
            try
            {
                var res = await _service.FinishOrder(orderId);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// Получение описания товаров в заказе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(ItemModelWithCount), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsByOrdeId(long id)
        {
            try
            {
                var res = await _service.GetProducts(id);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                var errorResponse = new { ex.Message };
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
