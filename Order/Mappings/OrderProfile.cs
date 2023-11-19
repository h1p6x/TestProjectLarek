using AutoMapper;
using Order.Database.Entities;
using Order.Models;

namespace Order.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderItem, OrderItemModel>();
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<Orders, OrderModel>();
            CreateMap<OrderModel, Orders>();
        }
    }
}
