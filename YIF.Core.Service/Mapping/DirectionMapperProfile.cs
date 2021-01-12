using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Service.Mapping
{
    class DirectionMapperProfile : Profile
    {
        public DirectionMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<Direction, DirectionDTO>().ReverseMap();
            //CreateMap<DirectionDTO, DirectionApiModel>().ReverseMap();

            //.ForMember(dto => dto.Specialities, opt => opt.MapFrom(x => x.Specialities));
            //.ForMember(dto => dto.Roles, opt => opt.MapFrom<GetRolesResolver>());
            //CreateMap<DirectionDTO, Direction>()
            //    //.ForMember(dto => dto.Specialities, opt => opt.MapFrom(x => x.Specialities));
            //    .AfterMap<SetRolesResolver>();
            //CreateMap<UserDTO, UserApiModel>()
            //    .ReverseMap();
        }
    }
}
