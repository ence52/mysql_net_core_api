using mysql_net_core_api.Core.Enums;

namespace mysql_net_core_api.Core.Entitites
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public UserEntity? User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
