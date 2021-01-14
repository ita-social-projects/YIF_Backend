using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Domain.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IApplicationDbContext _context;

        public TokenRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateUserToken(DbUser user, string refreshToken)
        {
            if (user == null) return false;

            var tokendb = _context.Tokens.Find(user.Id);

            if (tokendb == null)
            {
                _context.Tokens.Add(new Token
                {
                    Id = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                });
            }
            else
            {
                tokendb.RefreshToken = refreshToken;
                tokendb.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                _context.Tokens.Update(tokendb);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
