﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mysql_net_core_api.Services.JWT
{
    public class JWTService : IJWTService
    { private readonly IConfiguration _config;
        public JWTService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(string username, List<Claim> extraClaims = null)
        {
            var jwtSettings = _config.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub,username),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
           };

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),signingCredentials:creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
