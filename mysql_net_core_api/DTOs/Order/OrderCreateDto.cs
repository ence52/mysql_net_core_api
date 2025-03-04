using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Enums;
using mysql_net_core_api.DTOs.OrderItem;
using mysql_net_core_api.DTOs.User;

namespace mysql_net_core_api.DTOs.Order
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; } 
        public ICollection<OrderItemCreateDto> OrderItems { get; set; }
    }
}
