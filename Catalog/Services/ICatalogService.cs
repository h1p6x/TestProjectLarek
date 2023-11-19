using Catalog.Models;

namespace Catalog.Services
{
    public interface ICatalogService
    {
        public Task<ItemModel> GetItem(long id);
        public Task<IEnumerable<ItemModel>> GetItemsByIds(IEnumerable<long> ids);
        public Task<IEnumerable<ItemModel>> GetItemsByName(string itemName);
        public Task<IList<ItemModel>> GetItemsByCategoryName(string categoryName);
        public Task<IList<ItemModel>> GetItemList();
        public Task<bool> Exists(long id, int count);
        public Task<IList<CategoryModel>> GetCategoryCatalog(long categoryId);
        public Task<ItemModel> CreateItem(ItemCreateModel newItem);
        public Task DeleteItem(long id);
        public Task<ItemModel> UpdateItem(long id, ItemCreateModel updatedItem);
        public Task<BrandModel> CreateBrand(BrandCreateModel newBrand);
        public Task<CategoryCreateModel> CreateCategory(CategoryCreateModel newCategory);
        public Task<IList<BrandModel>> GetCategoryBrands(long categoryId);
        public Task<IList<ItemModel>> GetItemsByBrandName(string brandName);
        public Task<ReservedItemModel> Reserve(IEnumerable<ReservedItemModel> items);
        public Task<IEnumerable<ReservedItemModel>> CancelReservation(IEnumerable<ReservedItemModel> items);
        public Task<IEnumerable<ReservedItemModel>> GetReservedItemList();
        public Task<IEnumerable<ReservedItemModel>> RemoveReservationItem(IEnumerable<ReservedItemModel> items);
    }
}
