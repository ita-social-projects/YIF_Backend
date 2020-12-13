using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IRepository<DbUser, UserDTO>
    {
        private IApplicationDbContext _db;

        public UserRepository(IApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<UserDTO> Create(DbUser user)
        {
            if (user != null)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<DbUser, UserDTO>()));
                return mapper.Map<UserDTO>(await _db.Users.FindAsync(user));
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
                    _db.SaveChanges();
                    var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<DbUser, UserDTO>()));
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
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<UserDTO>> Find(Expression<Func<DbUser, bool>> predicate)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<DbUser, UserDTO>();
            });
            var mapper = new Mapper(configuration);

            return mapper.Map<IEnumerable<UserDTO>>(await _db.Users.Where(predicate).ToListAsync());
        }

        public async Task<UserDTO> Get(string id)
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<DbUser, UserDTO>()));
            return mapper.Map<UserDTO>(await _db.Users.FindAsync(id));
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<DbUser, UserDTO>();
            }));

            return mapper.Map<IEnumerable<UserDTO>>(await _db.Users.ToListAsync());
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}