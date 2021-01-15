using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Data.Interfaces;

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

            CreateMap<UniversityDTO, UniversityFilterResponseApiModel>()
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

            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User == null ? "unknown" : src.User.PhoneNumber));

            CreateMap<UserProfileDTO, UserProfile>()
                .AfterMap<SetUserInUserProfileResolver>();

            CreateMap<UserProfileDTO, UserProfileApiModel>()
                .AfterMap<SetSchoolInUserProfileApiModelResolver>();

            //CreateMap<UserProfileApiModel, UserProfileDTO>();
            //CreateMap<UserProfileApiModel, UserProfileDTO>()
            //    .AfterMap<SetOtherFieldsInUserProfileDtoResolver>();

            CreateMap<UserProfileApiModel, UserProfile>();
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

    public class SetUserInUserProfileResolver : IMappingAction<UserProfileDTO, UserProfile>
    {
        private static UserManager<DbUser> _manager;
        public SetUserInUserProfileResolver(UserManager<DbUser> manager)
        {
            _manager = manager;
        }
        public void Process(UserProfileDTO profileDTO, UserProfile profile, ResolutionContext context)
        {
            profile.User = _manager.FindByIdAsync(profileDTO.Id).Result;
        }
    }

    public class SetSchoolInUserProfileApiModelResolver : IMappingAction<UserProfileDTO, UserProfileApiModel>
    {
        private static IApplicationDbContext _context;
        public SetSchoolInUserProfileApiModelResolver(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Process(UserProfileDTO profileDTO, UserProfileApiModel profile, ResolutionContext context)
        {
            profile.SchoolName = _context.Graduates.FirstOrDefault(x => x.UserId == profileDTO.Id)?.School.Name;
        }
    }

    public class SetOtherFieldsInUserProfileDtoResolver : IMappingAction<UserProfileApiModel, UserProfileDTO>
    {
        private static IApplicationDbContext _context;
        public SetOtherFieldsInUserProfileDtoResolver(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Process(UserProfileApiModel profileApi, UserProfileDTO profileDTO, ResolutionContext context)
        {
            var profile = _context.UserProfiles.FirstOrDefault(x => x.User.Email == profileApi.Email);
            profileDTO.Id = profile.Id;
            //profileDTO. = profile.DateOfBirth;
            profileDTO.DateOfBirth = profile.DateOfBirth;




        }
    }
}
