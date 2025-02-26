using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.DTOs.User;
using mysql_net_core_api.Repositories;
using mysql_net_core_api.Services.JWT;

namespace mysql_net_core_api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private  readonly ICacheService _cacheService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IConfiguration config, ICacheService cache, ILogger<AuthService> logger, IJWTService jwtService, IMapper mapper)
        {
            _cacheService = cache;
            _userRepository = userRepository;
            _config = config;
            _logger = logger;
            _jwtService = jwtService;
            _mapper = mapper;
        }


        public async Task<string> AuthenticateAsync(UserAuthDto userAuthDto)
        {
            _logger.LogInformation("User getting from db with username: {username}", userAuthDto.Email);
           var user = await _userRepository.GetUserByUsernameAsync(userAuthDto.Email);
            if (user == null || !VerifyPassword(userAuthDto.Password, user.PasswordHash)) {
                _logger.LogError("Wrong password or username");
                return null;
            }
            
            return _jwtService.GenerateToken(userAuthDto.Email);
        }

        public string HashPassword(string pasword)
        {
           return BCrypt.Net.BCrypt.HashPassword(pasword);
        }

        public async Task<bool> RegisterAsync(UserRegisterDto registerDto)
        {
            _logger.LogInformation("Checking username {username}", registerDto.Email);
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Email);
            if (existingUser != null) {
                _logger.LogWarning("This email exists! {username}", registerDto.Email);
                return false;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            _logger.LogInformation("Password hashed");
            var newUser = _mapper.Map<UserEntity>(registerDto);
            newUser.PasswordHash= hashedPassword;
            await _userRepository.AddAsync(newUser);
            _logger.LogInformation($"User added to db: {registerDto.Email}");
            await _cacheService.SetAsync($"User_{newUser.Id}", newUser, TimeSpan.FromMinutes(5));
            _logger.LogInformation($"User added to cache: {registerDto.Email}");
            _logger.LogInformation($"User registered: {registerDto.Email}");
            return true;
        }

        public bool VerifyPassword(string pasword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(pasword, hashedPassword);
        }
    }
}
