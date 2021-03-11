using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Shared;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IUserRepository<DbUser, UserDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public UserRepository(
            IApplicationDbContext context,
            IMapper mapper,
            UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> Create(DbUser dbUser, Object entityUser, string userPassword, string role)
        {
            IdentityResult result = null;
            if(dbUser == null)
            {
                throw new ArgumentNullException("dbUser");
            }

            if(userPassword == null)
            {
                result = await _userManager.CreateAsync(dbUser);
            }
            else
            {
                result = await _userManager.CreateAsync(dbUser, userPassword);
            }

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(dbUser, new List<string>() { role, ProjectRoles.BaseUser});
                if (entityUser != null)
                {
                    await _context.AddAsync(entityUser);
                }
                await _context.SaveChangesAsync();
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

        public async Task<DbUser> GetUserWithToken(string userId)
        {
            return await _userManager.Users.Include(u => u.Token).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<UserDTO> GetUserWithUserProfile(string userId)
        {
            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
            if (user?.UserProfile == null)
            {
                user.UserProfile = new UserProfile { Id = userId, RegistrationDate = DateTime.Now };
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> UpdateUserPhoto(UserDTO user, string photo)
        {
            if (user == null || string.IsNullOrWhiteSpace(photo)) return false;

            var userProfile = _context.UserProfiles.Find(user.Id);

            if (userProfile == null)
            {
                userProfile = new UserProfile { Id = user.Id, RegistrationDate = DateTime.Now };
            }

            userProfile.Photo = photo;
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            DbUser user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsDeleted = true;
                var token = _context.Tokens.Find(id);
                if (token != null) _context.Tokens.Remove(token);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<UserDTO>> Find(Expression<Func<DbUser, bool>> predicate)
        {
            var user = await _context.Users.Where(predicate).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(user);
        }

        public async Task<UserDTO> Get(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var list = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(list);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> Exist(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user != null;
        }
    }
}