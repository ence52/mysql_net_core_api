using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.DTOs.User;


namespace mysql_net_core_api.Services.User
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserRegisterDto dto);
        Task DeleteUserById(Guid id);
        Task<ICollection<OrderDto>> GetOrdersAsync(string email);

    }
}
