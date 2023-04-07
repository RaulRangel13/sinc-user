using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthSettings _authSettings;

        public TokenService(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public Task<AuthResponse> GenerateTokenAsync(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();            
            var expire = DateTime.UtcNow.AddHours(Convert.ToDouble(_authSettings.ExpireIn));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey));

            var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = "userapp",
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = expire,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new GenericIdentity(customer.Email, JwtBearerDefaults.AuthenticationScheme), new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString())
                })
            });

            var response = new AuthResponse()
            {
                Token = tokenHandler.WriteToken(securityToken),
                ExpireIn = Convert.ToInt32(_authSettings.ExpireIn),
                Type = JwtBearerDefaults.AuthenticationScheme
            };

            return Task.FromResult(response);
        }
    }
}
