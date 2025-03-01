using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.DTOs.Product;
using mysql_net_core_api.Repositories;

namespace mysql_net_core_api.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductEntity> _repo;
        private readonly ILogger<ProductService> _logger;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;
        public ProductService(IRepository<ProductEntity> repo, ILogger<ProductService> logger, ICacheService cache, IMapper mapper)
        {
            _cache=cache;
            _repo=repo;
            _logger=logger;
            _mapper=mapper;
        }
        public async Task<ProductEntity> CreateProductAsync(ProductCreateDto dto)
        {
            try
            {
                var product = _mapper.Map<ProductEntity>(dto);
                await _repo.AddAsync(product);
                _logger.LogInformation("Product added to db with id: {id}", product.Id);
                string cacheKey=$"Product_{product.Id}";
                await _cache.SetAsync(cacheKey, product,TimeSpan.FromMinutes(5));
                _logger.LogInformation("Product added to cache with id: {id}", product.Id);
                return product;
            }
            catch (Exception ex )
            {
                _logger.LogError("An error occured while creating product. {ex}", ex);
                throw;
            }
        }

        public async Task DeleteProductById(Guid id)
        {
            try
            {
                string cacheKey = $"Product_{id}";
                await _cache.RemoveAsync(cacheKey);
                _logger.LogInformation("Product removed from cache with id: {id}",id);
                var product = _mapper.Map<ProductEntity>(id);
                await _repo.DeleteAsync(product);
                _logger.LogInformation("Product removed from db with id: {id}", id);
            }
            catch (Exception ex )
            {
                _logger.LogError("An error occured while deleting product. {ex}", ex);
                throw;
            }
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            try
            {
                var products = await _repo.GetAllAsync();
                var newProducts = _mapper.Map<List<ProductDto>>(products);
                _logger.LogInformation("All Products getting from db ");
                return newProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting all products. {ex}", ex);
                throw;
            }
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            try
            {
                string cacheKey = $"Product_{id}";
                var product = await _cache.GetAsync<ProductEntity>(cacheKey);
                _logger.LogInformation("Product getting from cache with id: {id}", id);
                if (product != null)
                {
                    _logger.LogInformation("Product found in cache with id: {id}", id);
                    return _mapper.Map<ProductDto>(product);
                }

                var dbProduct = await _repo.GetByIdAsync(id);
                _logger.LogInformation("Product getting from db with id: {id}", id);
                await _cache.SetAsync(cacheKey, dbProduct,TimeSpan.FromMinutes(5));
                _logger.LogInformation("Product added to cache with id: {id}", id);
                return _mapper.Map<ProductDto>(dbProduct);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting product with id:{id}. {ex}",id,ex);
                throw;
            }
        }

        public async Task UpdateProductAsync(Guid id, ProductCreateDto dto)
        {
            try
            {
                var product = _mapper.Map<ProductEntity>(dto);
                product.Id = id;
                await _repo.UpdateAsync(product);
                _logger.LogInformation("Product updated in db with id: {id}", id);
                string cacheKey = $"Product_{id}";
                await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Product updated in cache with id: {id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating product with id:{id}. {ex}", id, ex);
                throw;
            }
        }
    }
}
