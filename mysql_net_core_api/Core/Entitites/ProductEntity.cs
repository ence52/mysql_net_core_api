using System.ComponentModel.DataAnnotations.Schema;

namespace mysql_net_core_api.Core.Entitites
{
    public class ProductEntity:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CategoryEntity? Category { get; set; }
    }
}
