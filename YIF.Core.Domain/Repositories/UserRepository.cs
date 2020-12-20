using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IRepository<DbUser, UserDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<DbUser> _userManager;


        public UserRepository(IApplicationDbContext context, UserManager<DbUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> Create(DbUser dbUser,Object entityUser,string userPassword)
        {
            var result = await _userManager.CreateAsync(dbUser, userPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(dbUser,ProjectRoles.Graduate);

                await _context.AddAsync(entityUser);
                await _context.SaveChangesAsync();

                var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<DbUser, UserDTO>()));
                return string.Empty;
            }

            return result.Errors.First().Description;
        }

        public async Task<bool> Update(DbUser user)
        {
            if (user != null)
            {
                if (_context.Users.Find(user) != null)
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            DbUser user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<UserDTO>> Find(Expression<Func<DbUser, bool>> predicate)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<DbUser, UserDTO>();
            });
            var mapper = new Mapper(configuration);
            return mapper.Map<IEnumerable<UserDTO>>(await _context.Users.Where(predicate).ToListAsync());
        }

        public async Task<UserDTO> Get(string id)
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<DbUser, UserDTO>()));
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return mapper.Map<UserDTO>(user);
            }
            throw new KeyNotFoundException("User not found:  " + id);
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<DbUser, UserDTO>();
            }));
            var list = await _context.Users.ToListAsync();
            return mapper.Map<IEnumerable<UserDTO>>(list);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}