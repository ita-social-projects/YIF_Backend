using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IRepository<DbUser, UserDTO>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(IApplicationDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> Create(DbUser user)
        {
            if (user != null)
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                var foundUser = await _db.Users.FindAsync(user);
                return _mapper.Map<UserDTO>(foundUser);
            }
            return null;
        }

        public async Task<bool> Update(DbUser user)
        {
            if (user != null)
            {
                if (_db.Users.Find(user) != null)
                {
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            DbUser user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<UserDTO>> Find(Expression<Func<DbUser, bool>> predicate)
        {
            var user = await _db.Users.Where(predicate).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(user);
        }

        public async Task<UserDTO> Get(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            throw new KeyNotFoundException("User not found:  " + id);
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var list = await _db.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(list);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}