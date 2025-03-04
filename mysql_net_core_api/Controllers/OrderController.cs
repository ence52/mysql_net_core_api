using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.Services.Order;

namespace mysql_net_core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {

            try
            {
                if (id == Guid.Empty)
                {
                    BadRequest("ID is required");
                }
                var order = await _service.GetByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting order. {ex}", ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    BadRequest("Order data is required");
                }
                await _service.CreateOrderAsync(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating order. {ex}", ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                if (id==Guid.Empty)
                {
                    BadRequest();
                }
                await _service.DeleteOrderByIdAsync(id);
                _logger.LogInformation("Order deleted");
                return NoContent();
            }
            catch (Exception ex )
            {
                _logger.LogError("An error occured while deleting order. {ex}", ex.Message);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Guid id , OrderCreateDto dto)
        {
            try
            {
                if (id==Guid.Empty||dto==null)
                {
                    BadRequest();
                }
                await _service.UpdateOrderAsync(id, dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating order. {ex}",ex.Message);
                throw;
            }
        }
    }
}
