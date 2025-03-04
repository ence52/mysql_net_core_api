using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Enums;
using mysql_net_core_api.DTOs.OrderItem;
using mysql_net_core_api.DTOs.User;

namespace mysql_net_core_api.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItemDto> OrderItems { get; set; } 
    }
}
