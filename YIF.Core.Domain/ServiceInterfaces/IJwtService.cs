using System.Collections.Generic;
using System.Security.Claims;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IJwtService
    {
        IEnumerable<Claim> SetClaims(DbUser user);
        string CreateToken(IEnumerable<Claim> claims);
        string CreateRefreshToken();
        IEnumerable<Claim> GetClaimsFromExpiredToken(string token);
    }
}
