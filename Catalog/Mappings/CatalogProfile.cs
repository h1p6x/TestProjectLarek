using AutoMapper;
using Catalog.Database.Entities;
using Catalog.Models;

namespace Catalog.Mappings
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<ItemModel, Item>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand { Name = src.Brand }))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category }));

            CreateMap<Item, ItemModel>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();
            CreateMap<Brand, BrandModel>();
            CreateMap<BrandModel, Brand>();
            CreateMap<ReservedItemModel, ReservedItem>();
            CreateMap<ReservedItem, ReservedItemModel>();
        }
    }
}
