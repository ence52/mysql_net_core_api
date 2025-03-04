using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mysql_net_core_api.DTOs.User;
using mysql_net_core_api.Services.User;

namespace mysql_net_core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDto userDto)
        {
            _logger.LogInformation("Started proccessing CreateUser");
            try
            {
                if (userDto==null)
                {
                    _logger.LogWarning("Recieved null payload in CreateUser");
                    return BadRequest("User data is required");
                }

                var createdUser = await _service.CreateUserAsync(userDto);
                if (createdUser==null)
                {
                    _logger.LogWarning("Failed to created user: {email}", createdUser.Email);
                }

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing CreateUser");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                _logger.LogInformation("Finished processing CreateUser");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogInformation("Started proccessing GetUserById");
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {

                    _logger.LogWarning("Recieved null payload while getting user: {id}",id);
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed getting user: {id}, message:{ex}",id,ex.Message);
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            if (users==null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            try
            {
                await _service.DeleteUserById(id);
                _logger.LogInformation("User deleted by id: {id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while removing user {id}: {ex.Message}");
                throw;
            }
        }
        

        
    }
}
