using AutoMapper;
using System;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Domain.Repositories
{
    public class TokenRepository : ITokenRepository<TokenDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TokenRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddUserToken(TokenDTO token)
        {
            var tokenDb = _mapper.Map<Token>(token);
            _context.Tokens.Add(tokenDb);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<TokenDTO> FindUserToken(string userId)
        {
            var tokenDb = await _context.Tokens.FindAsync(userId);
            return _mapper.Map<TokenDTO>(tokenDb);
        }

        public async Task<bool> UpdateUserToken(string userId, string refreshToken)
        {
            var tokenDb = _context.Tokens.Find(userId);
            tokenDb.RefreshToken = refreshToken;
            tokenDb.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _context.Tokens.Update(tokenDb);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
