namespace mysql_net_core_api.DTOs.OrderItem
{
    public class OrderItemDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
