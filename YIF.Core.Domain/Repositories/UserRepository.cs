using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IRepository<DbUser, UserDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public UserRepository(IApplicationDbContext context,
                              IMapper mapper,
                              UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> Create(DbUser dbUser, Object entityUser, string userPassword, string role)
        {
            var result = await _userManager.CreateAsync(dbUser, userPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(dbUser, role);
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

        public async Task<DbUser> GetUserWithUserProfile(string userId)
        {
            await SetDefaultUserProfileIfEmpty(userId);
            return await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> SetDefaultUserProfileIfEmpty(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return false;

            var userProfile = _context.UserProfiles.Find(userId);
            if (userProfile == null)
            {
                _context.UserProfiles.Add(new UserProfile
                {
                    Id = userId,
                    Name = "unknown",
                    MiddleName = "unknown",
                    Surname = "unknown",
                    DateOfBirth = null,
                    RegistrationDate = DateTime.Now,
                    Photo = null
                });
            }

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<bool> UpdateUserPhoto(DbUser user, string photo)
        {
            if (user == null || string.IsNullOrWhiteSpace(photo)) return false;

            var userProfile = _context.UserProfiles.Find(user.Id);

            if (userProfile == null)
            {
                _context.UserProfiles.Add(new UserProfile
                {
                    Id = user.Id,
                    Name = "unknown",
                    MiddleName = "unknown",
                    Surname = "unknown",
                    DateOfBirth = null,
                    RegistrationDate = DateTime.Now,
                    Photo = photo
                });
            }
            else
            {
                userProfile.Photo = photo;
                _context.UserProfiles.Update(userProfile);
            }

            await _context.SaveChangesAsync();
            return true;
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
            var user = await _context.Users.Where(predicate).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(user);
        }

        public async Task<UserDTO> Get(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            throw new KeyNotFoundException("User not found:  " + id);
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _context.Users.FindAsync(email);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            throw new KeyNotFoundException("User not found:  " + email);
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
    }
}