using Catalog.Models;
using Catalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly ILogger<CatalogController> _logger;
		private readonly ICatalogService _service;
		private readonly IConfiguration _configuration;
		public CatalogController(ILogger<CatalogController> logger, ICatalogService service)
		{
			_logger = logger;
			_service = service;
		}

		/// <summary>
		/// Возвращает список всех доступных товаров
		/// </summary>
		/// <returns></returns>
		[HttpGet("[action]")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetItemList()
		{
			try
			{
				var res = await _service.GetItemList();
				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while processing the request.");
				return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
			}
		}

		/// <summary>
		/// Получение товара по id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("[action]/{id}")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetItem(long id)
		{
			try
			{
				var res = await _service.GetItem(id);
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
		/// Получение информации о нескольких товарах по id
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByIds([FromQuery] IEnumerable<long> ids)
        {
            try
            {
                var res = await _service.GetItemsByIds(ids);
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
        /// Проверить существует ли продукт в каталоге в необходимом количестве
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ItemExists(long id, [BindRequired] int itemCount)
        {
            try
            {
                var res = await _service.Exists(id, itemCount);
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
		/// Получение товаров по имени
		/// </summary>
		/// <param name="itemName"></param>
		/// <returns></returns>
        [HttpGet("[action]")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetItemsByName([BindRequired] string itemName)
		{
			try
			{
				var res = await _service.GetItemsByName(itemName);
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
		/// Получение всех товаров по имени бренда
		/// </summary>
		/// <param name="brandName"></param>
		/// <returns></returns>
		[HttpGet("[action]")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetItemsByBrandName([BindRequired] string brandName)
		{
			try
			{
				var res = await _service.GetItemsByBrandName(brandName);
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
		/// Получение всех товаров для указанной категории
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		[HttpGet("[action]")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetItemsByCategoryName([BindRequired] string categoryName)
		{
			try
			{
				var res = await _service.GetItemsByCategoryName(categoryName);
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
		/// Возвращает информацию о категории по id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("[action]/{id}")]
		[ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetCategory(long id)
		{
			try
			{
				var res = await _service.GetCategoryCatalog(id);
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
		/// Создание нового товара
		/// </summary>
		/// <param name="newItem"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateItem([FromBody] ItemCreateModel newItem)
		{
			try
			{
				var createdItem = await _service.CreateItem(newItem);
				return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
			}
			catch (ArgumentException ex)
			{
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
		/// Удаление существующего товара
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("[action]/{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteItem(long id)
		{
			try
			{
				await _service.DeleteItem(id);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
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
		/// Изменение существующего товара
		/// </summary>
		/// <param name="id"></param>
		/// <param name="updatedItem"></param>
		/// <returns></returns>
		[HttpPut("[action]/{id}")]
		[ProducesResponseType(typeof(ItemModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateItem(long id, [FromBody] ItemCreateModel updatedItem)
		{
			try
			{
				var existingItem = await _service.GetItem(id);
				if (existingItem == null)
				{
					return BadRequest($"Товар с ID {id} не найден.");
				}
				var updatedResult = await _service.UpdateItem(id, updatedItem);
				return CreatedAtAction(nameof(GetItem), new { id = updatedResult.Id }, updatedResult);
			}
			catch (ArgumentException ex)
			{
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
		/// Создание бренда
		/// </summary>
		/// <param name="newBrand"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		[ProducesResponseType(typeof(BrandModel), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateBrand([FromBody] BrandCreateModel newBrand)
		{
			try
			{
				var createdBrand = await _service.CreateBrand(newBrand);
				return CreatedAtAction(nameof(GetItem), new { id = createdBrand.Id }, createdBrand);
			}
			catch (ArgumentException ex)
			{
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
		/// Создание категории
		/// </summary>
		/// <param name="newCategory"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		[ProducesResponseType(typeof(CategoryCreateModel), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateModel newCategory)
		{
			try
			{
				var createdCategory = await _service.CreateCategory(newCategory);
				return Ok(createdCategory);
			}
			catch (ArgumentException ex)
			{
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
		/// Резервация товара
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
        [HttpPost("[action]/Add")]
        [ProducesResponseType(typeof(ReservedItemModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Reserve([FromBody] IEnumerable<ReservedItemModel> items)
        {
            try
            {
                var createdReservedItem = await _service.Reserve(items);
                return Ok(createdReservedItem);
            }
            catch (ArgumentException ex)
            {
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
		/// Отмена резервации
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ReservedItemModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelReserve([FromBody] IEnumerable<ReservedItemModel> items)
        {
            try
            {
                var cancelReservedItem = await _service.CancelReservation(items);
                return Ok(cancelReservedItem);
            }
            catch (ArgumentException ex)
            {
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
		/// Удалить резервацию, когда заказ завершен
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ReservedItemModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveReserve([FromBody] IEnumerable<ReservedItemModel> items)
        {
            try
            {
                var cancelReservedItem = await _service.RemoveReservationItem(items);
                return Ok(cancelReservedItem);
            }
            catch (ArgumentException ex)
            {
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
        /// Получение всех зарезервированных товаров
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ReservedItemModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReservedItemList()
        {
            try
            {
                var res = await _service.GetReservedItemList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
