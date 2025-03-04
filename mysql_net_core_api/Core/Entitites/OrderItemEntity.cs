using System.ComponentModel.DataAnnotations.Schema;

namespace mysql_net_core_api.Core.Entitites
{
    public class OrderItemEntity:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public OrderEntity? Order { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
