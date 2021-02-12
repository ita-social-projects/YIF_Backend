using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Shared;

namespace YIF.Core.Domain.Repositories
{
    public class UserProfileRepository : IUserProfileRepository<UserProfile, UserProfileDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public UserProfileRepository(IApplicationDbContext context,
                              IMapper mapper,
                              UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public void Dispose() => _context.Dispose();

        public async Task<UserProfile> GetDefaultUserProfile(string userId)
        {
            return await Task.FromResult(new UserProfile
            {
                Id = userId,
                RegistrationDate = DateTime.Now
            });
        }

        public async Task<UserProfile> SetDefaultUserProfileIfEmpty(string userId)
        {
            var profile = new UserProfile
            {
                Id = userId,
                RegistrationDate = DateTime.Now
            };
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<bool> EmailExistInAnotherUser(string email, string currentUserId)
        {
            var anotherUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return anotherUser != null && anotherUser?.Id != currentUserId;
        }

        public async Task<bool> IsCurrentUserTheGraduate(string currentUserId)
        {
            var graduate = await _context.Graduates.FirstOrDefaultAsync(x => x.UserId == currentUserId);
            return graduate != null;
        }

        public async Task<bool> SetSchoolForGraduate(string schoolName, string currentUserId)
        {
            var graduate = await _context.Graduates.Include(g => g.School).FirstOrDefaultAsync(x => x.UserId == currentUserId);
            if (graduate.School?.Name != schoolName)
            {
                var school = await _context.Schools.FirstOrDefaultAsync(x => x.Name == schoolName);
                graduate.SchoolId = school?.Id;
                _context.Graduates.Update(graduate);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserProfileDTO> SetUserProfile(UserProfileDTO profileDto)
        {
            // Get current user
            var user = _userManager.Users.Include(u => u.UserProfile).FirstOrDefault(x => x.Id == profileDto.Id);

            // Set new user profile and add changes
            var profile = _mapper.Map<UserProfile>(profileDto);
            profile.User = user;
            profile.DateOfBirth = user.UserProfile?.DateOfBirth;
            profile.RegistrationDate = user.UserProfile == null ? DateTime.MinValue : user.UserProfile.RegistrationDate;
            profile.Photo = user.UserProfile?.Photo;

            // Save changes
            user.PhoneNumber = profileDto.PhoneNumber;
            user.Email = profileDto.Email;
            user.UserProfile = profile;
            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserProfileDTO>(profile);
        }
    }
}
