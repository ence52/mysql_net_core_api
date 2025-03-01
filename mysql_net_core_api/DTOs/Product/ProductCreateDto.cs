using System.ComponentModel.DataAnnotations;

namespace mysql_net_core_api.DTOs.Product
{
    public class ProductCreateDto
    {

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        
    }
}
