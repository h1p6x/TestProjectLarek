using Delivery.Database.Entities;
using Delivery.Models;
using Delivery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly ILogger<DeliveryController> _logger;
        private readonly IDeliveryService _service;
        
        public DeliveryController(ILogger<DeliveryController> logger, IDeliveryService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Получение информации о заказе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}/info")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderInfoById(long id)
        {
            try
            {
                var res = await _service.GetOrderInfo(id);
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
        /// Получение информации о товарах в заказе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}/products")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsByOrderId(long id)
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

        /// <summary>
        /// Вернуть заказ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{id}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReturnOrder(long id)
        {
            try
            {
                var res = await _service.ReturnOrder(id);
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
        /// Завершить доставку заказа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{id}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FinishOrder(long id)
        {
            try
            {
                var res = await _service.FinishOrder(id);
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
        /// Добавить заказ для доставки
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(DeliveryItemModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecordOrder([FromBody] DeliveryItemModel item)
        {
            try
            {
                var res = await _service.RecordOrder(item);
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
