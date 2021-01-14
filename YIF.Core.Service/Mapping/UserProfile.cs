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

namespace YIF.Core.Service.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
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
}
