using mysql_net_core_api.DTOs.User;

namespace mysql_net_core_api.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRegisterDto dto);
        Task<string> AuthenticateAsync(UserAuthDto userAuthDto);
        string HashPassword(string pasword);
        bool VerifyPassword(string pasword,string hashedPassword);
    }
}
