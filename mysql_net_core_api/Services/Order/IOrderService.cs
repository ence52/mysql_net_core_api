using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.DTOs.Product;

namespace mysql_net_core_api.Services.Order
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(OrderCreateDto dto);
        Task UpdateOrderAsync(Guid id, OrderCreateDto dto);
        Task DeleteOrderByIdAsync(Guid id);
        
    }
}
