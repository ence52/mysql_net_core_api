using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mysql_net_core_api.DTOs.User;
using mysql_net_core_api.Services.Auth;
using mysql_net_core_api.Services.User;

namespace mysql_net_core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                bool isRegistered = await _authService.RegisterAsync(registerDto);
                if (!isRegistered)
                {
                    return BadRequest(new { message = "Email exists already!" });
                }
                _logger.LogInformation("User registered successfully: {email}", registerDto.Email);
                return Ok(new { message = "Registered Succesfully" });
            }
            catch (Exception ex )
            {
                _logger.LogError($"Error occured while registering:{ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] UserAuthDto userAuthDto)
        {
            try
            {
                string result = await _authService.AuthenticateAsync(userAuthDto);
                if (result == null)
                    return BadRequest(new {message= "Wrong Username or Password" });
                return Ok(new { type="Bearer", token=result});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login failed: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while logging in" });
            }
        }

    }
}
