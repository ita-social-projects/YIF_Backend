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
using YIF.Core.Data.Others;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IUserRepository<DbUser, UserDTO, UserProfile, UserProfileDTO>
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

        public async Task<UserDTO> GetUserWithUserProfile(string userId)
        {
            var user = await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
            if (user?.UserProfile == null)
            {
                user.UserProfile = await GetDefaultUserProfile(userId);
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserProfile> GetDefaultUserProfile(string userId)
        {
            return await Task.FromResult(new UserProfile
            {
                Id = userId,
                Name = "unknown",
                MiddleName = "unknown",
                Surname = "unknown",
                Photo = null,
                DateOfBirth = null,
                RegistrationDate = DateTime.Now
            });
        }

        public async Task<UserProfile> SetDefaultUserProfileIfEmpty(string userId)
        {
            var profile = new UserProfile
            {
                Id = userId,
                Name = "unknown",
                MiddleName = "unknown",
                Surname = "unknown",
                DateOfBirth = null,
                RegistrationDate = DateTime.Now,
                Photo = null
            };
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<UserProfileDTO> SetUserProfile(UserProfileDTO profileDto, string schoolName = null)
        {
            // Check email (Is newEmail exist in another user or not)
            var anotherUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == profileDto.Email);
            if (anotherUser != null && anotherUser?.Id != profileDto.Id)
            {
                throw new ArgumentException("Електронна пошта вже використовується:  " + profileDto.Email);
            }

            // Get current user
            var user = _userManager.Users.Include(u => u.UserProfile).FirstOrDefault(x => x.Id == profileDto.Id);
            if (user == null) throw new KeyNotFoundException("Користувача не знайдено із таким id:  " + profileDto.Id);

            // Set school
            if (!string.IsNullOrWhiteSpace(schoolName))
            {
                var graduate = await _context.Graduates.FirstOrDefaultAsync(x => x.UserId == profileDto.Id);
                if (graduate == null) throw new ArgumentException("Користувач не належить до ролі: " + ProjectRoles.Graduate +
                    ". Поле 'schoolname' не потрібно заповняти для цього коритувача.");

                if (graduate.School?.Name != schoolName)
                {
                    var school = await _context.Schools.FirstOrDefaultAsync(x => x.Name == schoolName);
                    if (school == null) throw new ArgumentException("Не знайдено зазначеної школи:  " + schoolName);
                    graduate.SchoolId = school.Id;
                    _context.Graduates.Update(graduate);
                }
            }

            // Set new user profile and add changes
            var profile = _mapper.Map<UserProfile>(profileDto);
            profile.User = user;
            profile.DateOfBirth = user.UserProfile?.DateOfBirth;
            profile.RegistrationDate = user.UserProfile == null ? DateTime.MinValue : user.UserProfile.RegistrationDate;
            user.PhoneNumber = profileDto.PhoneNumber;
            user.Email = profileDto.Email;

            // Save changes
            user.UserProfile = profile;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserProfileDTO>(profile);
        }

        public async Task<bool> UpdateUserPhoto(UserDTO user, string photo)
        {
            if (user == null || string.IsNullOrWhiteSpace(photo)) return false;

            var userProfile = _context.UserProfiles.Find(user.Id);

            if (userProfile == null)
            {
                userProfile = await SetDefaultUserProfileIfEmpty(user.Id);
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
    }
}