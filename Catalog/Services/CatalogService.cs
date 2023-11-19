using AutoMapper;
using Catalog.Database;
using Catalog.Database.Entities;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services
{
	public class CatalogService : ICatalogService
	{
		private readonly CatalogDbContext _dbContext;
		private readonly IMapper _mapper;

		public CatalogService(CatalogDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<ItemModel> GetItem(long id)
		{
			var item = await _dbContext.Items
						.Include(i => i.Brand)
						.Include(i => i.Category)
						.SingleOrDefaultAsync(i => i.Id == id);
			if (item is null)
				throw new ArgumentException($"Товар с id = {id} не найден");

			return _mapper.Map<ItemModel>(item);
		}

        public async Task<IEnumerable<ItemModel>> GetItemsByIds(IEnumerable<long> ids)
        {
            var items = await _dbContext.Items
                        .Include(i => i.Brand)
                        .Include(i => i.Category)
                        .Where(i => ids.Contains(i.Id))
                        .ToListAsync();

            if (items.Count == 0)
                throw new ArgumentException("Товары с указанными id не найдены");

            return _mapper.Map<IEnumerable<ItemModel>>(items);
        }


        public async Task<IEnumerable<ItemModel>> GetItemsByName(string itemName)
		{
			var items = await _dbContext.Items
								.Include(b => b.Brand)
								.Include(c => c.Category)
								.ToListAsync();

			var lowerItemName = itemName.ToLower();
			var item = items.Where(n => n.Name.ToLower().Contains(lowerItemName))
							.ToList();

			if (item is null)
				throw new ArgumentException($"Товар с name = {itemName} не найден");

			return _mapper.Map<IEnumerable<ItemModel>>(item);
		}

		public async Task<IList<ItemModel>> GetItemList()
		{
			var items = await _dbContext.Items
						.Include(i => i.Brand)
						.Include(i => i.Category)
						.ToListAsync();
			return _mapper.Map<List<ItemModel>>(items);
		}

        public async Task<bool> Exists(long id, int count)
        {
            var product = await GetItem(id);
            if (product is null)
                return false;

            return product.Count >= count;
        }

        public async Task<IList<ItemModel>> GetItemsByBrandName(string brandName)
		{
			var items = await _dbContext.Items
								.Include(item => item.Category)
								.Include(item => item.Brand)
								.ToListAsync();

			var lowerBrandName = brandName.ToLower();
			var filteredItems = items.Where(item => item.Brand.Name.ToLower()
													.Contains(lowerBrandName)).ToList();

			return _mapper.Map<List<ItemModel>>(filteredItems);
		}

		public async Task<IList<ItemModel>> GetItemsByCategoryName(string categoryName)
		{
			var items = await _dbContext.Items
								.Include(item => item.Category)
								.Include(item => item.Brand)
								.ToListAsync();

			var lowerCategoryName = categoryName.ToLower();
			var filteredItems = items.Where(item => item.Category.Name.ToLower()
													.Contains(lowerCategoryName)).ToList();

			return _mapper.Map<List<ItemModel>>(filteredItems);
		}

		public async Task<ItemModel> CreateItem(ItemCreateModel newItem)
		{
			if (newItem == null)
				throw new ArgumentNullException(nameof(newItem));

			var category = await _dbContext.Categories
				.FirstOrDefaultAsync(c => c.Name == newItem.Category);

			if (category == null)
				throw new ArgumentException($"Категория с именем {newItem.Category} не найдена");

			var brand = await _dbContext.Brands
			   .FirstOrDefaultAsync(b => b.Name == newItem.Brand);

			if (brand == null)
				throw new ArgumentException($"Бренд с именем {newItem.Brand} не найден");
			
			var isAvailable = newItem.Count > 0;
			
			var itemEntity = new Item
			{
				Name = newItem.Name,
				Article = newItem.Article,
				Price = newItem.Price,
				Count = newItem.Count,
				Description = newItem.Description,
				Category = category,
				Brand = brand,
				IsAvailable = isAvailable
			};

			await _dbContext.Items.AddAsync(itemEntity);
			await _dbContext.SaveChangesAsync();

			var createdItem = _mapper.Map<ItemModel>(itemEntity);

			return createdItem;
		}

		public async Task<BrandModel> CreateBrand(BrandCreateModel newBrand)
		{
			if (newBrand == null)
				throw new ArgumentNullException(nameof(newBrand));

			if (await _dbContext.Brands.AnyAsync(b => b.Name == newBrand.Name))
				throw new ArgumentException($"Бренд с именем - {newBrand.Name} уже существует");

			var category = await _dbContext.Categories
				.FirstOrDefaultAsync(c => c.Name == newBrand.Category);

			if (category == null)
				throw new ArgumentException($"Категория с именем {newBrand.Category} не найдена");

			var brandEntity = new Brand
			{
				Name = newBrand.Name,
				Description = newBrand.Description,
				Category = category
			};

			await _dbContext.Brands.AddAsync(brandEntity);
			await _dbContext.SaveChangesAsync();

			var createdBrand = _mapper.Map<BrandModel>(brandEntity);

			return createdBrand;
		}

		public async Task<CategoryCreateModel> CreateCategory(CategoryCreateModel newCategory)
		{
			if (newCategory == null)
				throw new ArgumentNullException(nameof(newCategory));

			if (await _dbContext.Categories.AnyAsync(b => b.Name == newCategory.Name))
				throw new ArgumentException($"Категория с именем - {newCategory.Name} уже существует");

			var categoryEntity = new Category
			{
				Name = newCategory.Name,
				Description = newCategory.Description,
			};

			await _dbContext.Categories.AddAsync(categoryEntity);
			await _dbContext.SaveChangesAsync();

			var createdCategoryResponse = new CategoryCreateModel
			{
				Name = categoryEntity.Name,
				Description = categoryEntity.Description,
			};

			return createdCategoryResponse;
		}

		public async Task DeleteItem(long id)
		{
			var itemToRemove = await _dbContext.Items.FirstOrDefaultAsync(item => item.Id == id);
			if (itemToRemove == null)
			{
				throw new ArgumentException($"Товар с id = {id} не найден.");
			}

			_dbContext.Items.Remove(itemToRemove);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<ItemModel> UpdateItem(long id, ItemCreateModel updatedItem)
		{
			if (updatedItem == null)
				throw new ArgumentNullException(nameof(updatedItem));

			var itemToUpdate = await _dbContext.Items.FindAsync(id);

			if (itemToUpdate == null)
				throw new ArgumentException($"Товар с ID {id} не найден");

			var category = await _dbContext.Categories
				.FirstOrDefaultAsync(c => c.Name == updatedItem.Category);

			if (category == null)
				throw new ArgumentException($"Категория с именем {updatedItem.Category} не найдена");

			var brand = await _dbContext.Brands
				.FirstOrDefaultAsync(b => b.Name == updatedItem.Brand);

			if (brand == null)
				throw new ArgumentException($"Бренд с именем {updatedItem.Brand} не найден");

			itemToUpdate.Name = updatedItem.Name;
			itemToUpdate.Article = updatedItem.Article;
			itemToUpdate.Price = updatedItem.Price;
			itemToUpdate.Description = updatedItem.Description;
			itemToUpdate.Category = category;
			itemToUpdate.Brand = brand;
			itemToUpdate.IsAvailable = updatedItem.Count > 0;
			
			_dbContext.Items.Update(itemToUpdate);
			await _dbContext.SaveChangesAsync();

			var updatedItemModel = _mapper.Map<ItemModel>(itemToUpdate);

			return updatedItemModel;
		}


		public async Task<IList<CategoryModel>> GetCategoryCatalog(long categoryId)
		{
			var categoryEntity = await _dbContext.Categories
			.Where(c => c.Id == categoryId)
			.Include(c => c.Brands)
			.Include(c => c.Items)
			.FirstOrDefaultAsync();

			if (categoryEntity == null)
				throw new ArgumentException($"Категория с id = {categoryId} не найдена");

			var categoryModel = _mapper.Map<CategoryModel>(categoryEntity);

			var categories = new List<CategoryModel> { categoryModel };

			return categories;
		}

		public async Task<IList<BrandModel>> GetCategoryBrands(long categoryId)
		{
			var category = await _dbContext.Categories
				.Include(c => c.Brands)
				.Where(c => c.Id == categoryId)
				.FirstOrDefaultAsync();

			if (category == null)
				throw new ArgumentException($"Категория с id = {categoryId} не найдена");

			var brands = category.Brands.ToList();
			return _mapper.Map<List<BrandModel>>(brands);
		}

		public async Task<ReservedItemModel> Reserve(IEnumerable<ReservedItemModel> items)
		{
			ReservedItemModel reservedItem = null;

			foreach (var item in items)
			{
				var itemInfo = await _dbContext.Items
					.FirstOrDefaultAsync(i => i.Id == item.ItemId);

				if (itemInfo == null)
					throw new ArgumentException($"Товар с id = {item.ItemId} не найден");

				if (item.Count <= 0)
					throw new ArgumentException("Количество должно быть больше нуля");

				if (item.Count > itemInfo.Count)
					throw new ArgumentException($"Запрошенное количество товара с id = {item.ItemId} " +
					                            $"({item.Count}) больше, чем доступное количество ({itemInfo.Count}).");

				var existingReservedItem = await _dbContext.ReservedItems
					.FirstOrDefaultAsync(x => x.ItemId == item.ItemId);
				
				// Добавляем резервацию для существующей записи в бд
				if (existingReservedItem != null)
				{
					var entry = _dbContext.Entry(existingReservedItem);
					existingReservedItem.Count += item.Count;
					entry.Property(x => x.Count).IsModified = true;
					reservedItem = _mapper.Map<ReservedItemModel>(existingReservedItem);
				}
				else
				{
					var reservedItemEntity = _mapper.Map<ReservedItem>(item);
					await _dbContext.ReservedItems.AddAsync(reservedItemEntity);
					reservedItem = _mapper.Map<ReservedItemModel>(reservedItemEntity);
				}

				itemInfo.Count -= item.Count;
			}

			await _dbContext.SaveChangesAsync();

			return reservedItem;
		}


		public async Task<IEnumerable<ReservedItemModel>> CancelReservation(IEnumerable<ReservedItemModel> items)
		{
			var cancelledItems = new List<ReservedItemModel>();

			foreach (var item in items)
			{
				var dbItem = await _dbContext.ReservedItems.FirstOrDefaultAsync(x => x.ItemId == item.ItemId);

				if (dbItem == null)
					throw new ArgumentException($"Товар с id = {item.ItemId} не найден в резервации.");

				if (item.Count <= 0)
					throw new ArgumentException("Количество должно быть больше нуля");

				if (item.Count > dbItem.Count)
					throw new ArgumentException($"Запрошенное количество для отмены резервации " +
					                            $"товара с id = {item.ItemId} ({item.Count}) больше, чем " +
					                            $"зарезервированное количество ({dbItem.Count}).");

				dbItem.Count -= item.Count;

				if (dbItem.Count == 0)
					_dbContext.ReservedItems.Remove(dbItem);

				var ItemInfo = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == item.ItemId);

				if (ItemInfo == null)
					throw new ArgumentException($"Товар с id = {item.ItemId} не найден");

				ItemInfo.Count += item.Count;

				cancelledItems.Add(item);
			}

			await _dbContext.SaveChangesAsync();

			return cancelledItems;
		}

        public async Task<IEnumerable<ReservedItemModel>> RemoveReservationItem(IEnumerable<ReservedItemModel> items)
        {
            var removedItems = new List<ReservedItemModel>();

            foreach (var item in items)
            {
                var dbItem = await _dbContext.ReservedItems.FirstOrDefaultAsync(x => x.ItemId == item.ItemId);

                if (dbItem == null)
                    throw new ArgumentException($"Товар с id = {item.ItemId} не найден в резервации.");

                if (item.Count <= 0)
                    throw new ArgumentException("Количество должно быть больше нуля");

                if (item.Count > dbItem.Count)
                    throw new ArgumentException($"Запрошенное количество для удаления резервации " +
                                                $"товара с id = {item.ItemId} ({item.Count}) больше, " +
                                                $"чем зарезервированное количество ({dbItem.Count}).");

                dbItem.Count -= item.Count;

                if (dbItem.Count == 0)
                    _dbContext.ReservedItems.Remove(dbItem);
            }

            await _dbContext.SaveChangesAsync();

            return removedItems;
        }

        public async Task<IEnumerable<ReservedItemModel>> GetReservedItemList()
        {
            var items = await _dbContext.ReservedItems
                        .ToListAsync();
            return _mapper.Map<IEnumerable<ReservedItemModel>>(items);
        }

    }
}
 