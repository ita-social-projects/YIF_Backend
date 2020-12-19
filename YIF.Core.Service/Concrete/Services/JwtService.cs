using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IConfiguration _configuration;
        public JwtService(UserManager<DbUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string CreateTokenByUser(DbUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            roles = roles.OrderBy(x => x).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim("name", user.UserName),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretPhrase")));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(1),
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}