using mysql_net_core_api.Core.Enums;
using System.Security.Claims;

namespace mysql_net_core_api.Services.JWT
{
    public interface IJWTService
    {
        public string GenerateToken(string username, List<UserRoleEnum> roles);
    }
}
