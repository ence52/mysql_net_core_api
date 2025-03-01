using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Product;
using mysql_net_core_api.DTOs.User;

namespace mysql_net_core_api
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserRegisterDto, UserEntity>();

            CreateMap<ProductEntity,ProductDto>();
            CreateMap<ProductCreateDto, ProductEntity>();
        }
    }
}
