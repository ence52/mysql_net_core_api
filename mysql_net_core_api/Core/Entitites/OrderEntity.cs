using mysql_net_core_api.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace mysql_net_core_api.Core.Entitites
{
    public class OrderEntity:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } 
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public UserEntity? User { get; set; }
        [JsonIgnore]
        public ICollection<OrderItemEntity> OrderItems { get; set; }

        public decimal CalculateTotalAmount() =>TotalAmount= OrderItems?.Sum(item => { item.CalculateTotalPrice();
            return item.TotalPrice;
        })??0;
    }
}
