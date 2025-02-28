using Microsoft.AspNetCore.Mvc;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.DTOs.Product;
using mysql_net_core_api.DTOs.User;
using mysql_net_core_api.Services;

namespace mysql_net_core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
    {

        private readonly IService<Product> _service;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IService<Product> service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product=  await _service.GetByIdAsync(id);
            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogWarning("Recieved null payload in CreateProduct");
                    return BadRequest("Data is required");
                }
                await _service.AddAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing CreateProduct");
                return StatusCode(500, "Internal server error");
                throw;
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id) {
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Product deleted with id: {id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Product with id:{id}",id);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products from db.");
               var products=  await _service.GetAllAsync();

                if (products==null)
                {
                    NotFound();
                    _logger.LogWarning("No products found");
                }

                return Ok(products);
            }
            catch (Exception)
            {
                _logger.LogError("An error occured while getting all products from db.");
                throw;
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogWarning("Recieved null payload in Update");
                    return BadRequest("Data is required");
                }
                await _service.UpdateAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing UpdateProduct");
                return StatusCode(500, $"{ex}");
                throw;
            }
        }
    }
}
