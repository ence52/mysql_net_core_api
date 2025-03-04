using mysql_net_core_api.Core.Enums.Product;

namespace mysql_net_core_api.DTOs.Product
{
    public class ProductQuery
    {
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? MinPrice{ get; set; }
        public decimal? MaxPrice{ get; set; }
        public ProductSortByEnum? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
    