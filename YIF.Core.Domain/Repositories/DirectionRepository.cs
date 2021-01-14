using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class DirectionRepository : IRepository<Direction, DirectionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<string> Create(Direction dbUser, object entityUser, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> Create(Direction dbUser, object entityUser, string userPassword, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DirectionDTO>> Find(Expression<Func<Direction, bool>> predicate)
        {
            var directions = _context.Directions.Where(predicate).AsNoTracking().ToList();

            if (directions != null || directions.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<DirectionDTO>>(directions));
            }

            return null;
        }

        public Task<DirectionDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DirectionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DirectionDTO> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<DbUser> GetUserWithToken(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<DbUser> GetUserWithUserProfile(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Direction item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserPhoto(DbUser user, string photo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserToken(DbUser user, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
