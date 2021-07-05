using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class ShoolAdminMappers : Profile
    {
        public ShoolAdminMappers()
        {
            //CreateMap<SchoolAdmin, SchoolAdminDTO>().ReverseMap();
            CreateMap<SchoolAdmin, SchoolAdminDTO>()
               .ForMember(dst => dst.SchoolId, opt => opt.MapFrom(src => src.School.Id))
               .ForMember(dst => dst.SchoolName, opt => opt.MapFrom(src => src.School.Name))
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<School, SchoolDTO>().ReverseMap();
            CreateMap<SchoolModerator, SchoolModeratorDTO>().ReverseMap();
            CreateMap<SchoolDTO, SchoolOnlyNameResponseApiModel>().ReverseMap();
            CreateMap<SchoolAdminDTO, SchoolAdminResponseApiModel>().ReverseMap();
        }
    }
}
