using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SBapi.Common.Dto;
using SBapi.Service.Repository.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SBapi.Service.Repository.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync(UserRequest userRequest)
        {
            var username = _configuration["ApiSecurity:ClientId"];
            var password = _configuration["ApiSecurity:ClientSecret"];

            if (!(userRequest.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && userRequest.Password == password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
