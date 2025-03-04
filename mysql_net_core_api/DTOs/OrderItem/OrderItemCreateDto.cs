namespace mysql_net_core_api.DTOs.OrderItem
{
    public class OrderItemCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
