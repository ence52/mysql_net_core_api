using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mysql_net_core_api.DTOs.Product;
using mysql_net_core_api.Services.Product;

namespace mysql_net_core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _logger = logger;
            _service = service;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _service.GetAllAsync();
                _logger.LogInformation("Getting all products from db.");

                if (products == null)
                {
                    _logger.LogWarning("No products found in db");
                    return NotFound();
                }
                _logger.LogInformation($"{products.Count} products found");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all products. Message: {msg}", ex.Message);
                throw;
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            try
            {
                var product = await _service.GetByIdAsync(id);
                _logger.LogInformation("Getting product from db.");
                if (product == null)
                {
                    _logger.LogInformation("Product not found in db.");
                    return NotFound();
                }
                _logger.LogInformation("Product found in db.");
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting product. Message: {msg}", ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            try
            {
                if (dto==null)
                {
                    _logger.LogWarning("Recieved null payload in CreateProduct");
                    return BadRequest("Product data is required");
                }
               var createdProduct=  await _service.CreateProductAsync(dto);
                if (createdProduct == null)
                {
                    _logger.LogWarning("Failed to create product with id: {id}",createdProduct.Id);
                }
                _logger.LogInformation("Product created");
                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Recieved null payload in UpdateProduct");
                    return BadRequest("Product data is required");
                }
                await _service.UpdateProductAsync(id, dto);
                _logger.LogInformation("Product updated with id: {id}",id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                await _service.DeleteProductById(id);
                _logger.LogInformation("Product deleted by id: {id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured while deleting product with id:{id}.{ex}",id,ex.Message);
                throw;
            }
        }

        [HttpPost("query")]
        public async Task<IActionResult> GetFilteredProducts([FromBody]ProductQuery query)
        {
            var products = await _service.GetFilteredProductsAsync(query);
            return Ok(products);
        }
    }
}
