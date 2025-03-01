namespace mysql_net_core_api.Core.Entitites
{
    public class OrderItem:IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public Order? Order { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
