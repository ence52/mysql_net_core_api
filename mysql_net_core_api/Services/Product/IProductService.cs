using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Product;

namespace mysql_net_core_api.Services.Product
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductEntity> CreateProductAsync(ProductCreateDto dto);
        Task UpdateProductAsync(Guid id, ProductCreateDto dto);
        Task DeleteProductById(Guid id);
        Task<ICollection<ProductDto>> GetFilteredProductsAsync(ProductQuery query);
    }
}
