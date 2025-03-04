using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.DTOs.OrderItem;
using mysql_net_core_api.DTOs.Product;
using mysql_net_core_api.DTOs.User;

namespace mysql_net_core_api
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            //User
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserRegisterDto, UserEntity>();
            //Product
            CreateMap<ProductEntity,ProductDto>();
            CreateMap<ProductCreateDto, ProductEntity>();
            //Order
            CreateMap<OrderEntity, OrderDto>();
            CreateMap<OrderDto, OrderEntity>();

            CreateMap<OrderCreateDto, OrderDto>();
            CreateMap<OrderDto, OrderCreateDto>();

            CreateMap<OrderCreateDto, OrderEntity>();
            CreateMap<OrderEntity, OrderCreateDto>();
            //OrderItems
            CreateMap<OrderItemCreateDto, OrderItemEntity>();
            CreateMap<OrderItemEntity, OrderItemCreateDto>();

            CreateMap<OrderItemCreateDto, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemCreateDto>();

            CreateMap<OrderItemEntity,OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemEntity>();


        }
    }
}
