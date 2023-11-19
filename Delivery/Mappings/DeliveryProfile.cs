using AutoMapper;
using Delivery.Database.Entities;
using Delivery.Models;

namespace Delivery.Mappings
{
	public class DeliveryProfile : Profile
	{
		public DeliveryProfile() 
		{
			CreateMap<DeliveryItems, DeliveryItemModel>();
			CreateMap<DeliveryItemModel, DeliveryItems>();
		}
	}
}
