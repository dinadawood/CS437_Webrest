﻿using AutoMapper;
using WebRestEF.EF.Models;
using WebRestShared.DTO;

namespace WebRest.Code
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<AddressType, AddressTypeDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerAddress, CustomerAddressDTO>().ReverseMap();
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrdersLine, OrdersLineDTO>().ReverseMap();
            CreateMap<OrderState, OrderStateDTO>().ReverseMap();
            CreateMap<OrderStatus, OrderStatusDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductPrice, ProductPriceDTO>().ReverseMap();
            CreateMap<ProductStatus, ProductStatusDTO>().ReverseMap();
        }
    }
}
