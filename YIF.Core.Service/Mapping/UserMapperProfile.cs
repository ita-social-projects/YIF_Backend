using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;

namespace YIF.Core.Service.Mapping
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<DbUser, UserDTO>()
                .ForMember(dto => dto.Roles, opt => opt.MapFrom<GetRolesResolver>());
            CreateMap<UserDTO, DbUser>()
                .AfterMap<SetRolesResolver>();
            CreateMap<UserDTO, UserApiModel>()
                .ReverseMap();

            CreateMap<SpecialityToUniversity, SpecialityToUniversityDTO>()
                .ReverseMap();
            CreateMap<Speciality, SpecialityDTO>()
                .ReverseMap();
            CreateMap<University, UniversityDTO>()
                .ReverseMap();
            CreateMap<Direction, DirectionDTO>()
                .ReverseMap();
            CreateMap<DirectionToUniversity, DirectionToUniversityDTO>()
                .ReverseMap();
            CreateMap<UniversityAdmin, UniversityAdminDTO>()
                .ReverseMap();
            CreateMap<UniversityModerator, UniversityModeratorDTO>()
                .ReverseMap();
            CreateMap<Lecture, LectureDTO>()
                .ReverseMap();

            CreateMap<UniversityDTO, UniversityResponseApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));

            CreateMap<DbUser, UserProfileDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserProfile.Name))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.UserProfile.MiddleName))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.UserProfile.Surname))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.UserProfile.Photo))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfile.DateOfBirth))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.UserProfile.RegistrationDate));

            CreateMap<UserProfileDTO, UserProfileApiModel>();
            CreateMap<SchoolDTO, UserProfileApiModel>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UserProfileApiModel, UserProfile>()
                .ConvertUsing<GetExistingUserProfileResolver>();

            CreateMap<DbUser, UserProfileApiModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserProfile.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.UserProfile.Surname))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.UserProfile.MiddleName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .AfterMap<SetSchoolInUserProfileApiModelResolver>();

            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.PhoneNumber));
        }
    }

    public class GetRolesResolver : IValueResolver<DbUser, UserDTO, IEnumerable<string>>
    {
        private static UserManager<DbUser> _manager;
        public GetRolesResolver(UserManager<DbUser> manager)
        {
            _manager = manager;
        }
        public IEnumerable<string> Resolve(DbUser user, UserDTO userDTO, IEnumerable<string> roles, ResolutionContext context)
        {
            return _manager.GetRolesAsync(user).Result;
        }
    }

    public class SetRolesResolver : IMappingAction<UserDTO, DbUser>
    {
        private static UserManager<DbUser> _manager;
        public SetRolesResolver(UserManager<DbUser> manager)
        {
            _manager = manager;
        }
        public void Process(UserDTO userDTO, DbUser user, ResolutionContext context)
        {
            var roles = _manager.GetRolesAsync(user).Result;
            if (!roles.SequenceEqual(userDTO.Roles))
            {
                foreach (var role in roles)
                {
                    if (!userDTO.Roles.Contains(role))
                    {
                        _manager.RemoveFromRoleAsync(user, role);
                    }
                }
                _manager.AddToRolesAsync(user, userDTO.Roles);
            }
        }
    }

    public class SetSchoolInUserProfileApiModelResolver : IMappingAction<DbUser, UserProfileApiModel>
    {
        private static IApplicationDbContext _context;
        public SetSchoolInUserProfileApiModelResolver(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Process(DbUser user, UserProfileApiModel profile, ResolutionContext context)
        {
            profile.SchoolName = _context.Graduates.Include(s => s.School).FirstOrDefault(x => x.UserId == user.Id)?.School?.Name;
        }
    }

    public class GetExistingUserProfileResolver : ITypeConverter<UserProfileApiModel, UserProfile>
    {
        private static UserManager<DbUser> _userManager;
        public GetExistingUserProfileResolver(UserManager<DbUser> userManager)
        {
            _userManager = userManager;
        }

        public UserProfile Convert(UserProfileApiModel profileApi, UserProfile profile, ResolutionContext context)
        {
            var user = _userManager.Users.Include(u => u.UserProfile).FirstOrDefault(x => x.Email == profileApi.Email);
            user.PhoneNumber = profileApi.PhoneNumber;
            profile = user.UserProfile ?? new UserProfile() { RegistrationDate = DateTime.Now };
            profile.Id = user.Id;
            profile.Name = profileApi.Name;
            profile.Surname = profileApi.Surname;
            profile.MiddleName = profileApi.MiddleName;
            profile.User = user;
            return profile;
        }
    }
}
