using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.DtoModels.SchoolAdmin;
using YIF.Core.Domain.DtoModels.SchoolModerator;

namespace YIF.Core.Service.Mapping
{
    public class ShoolAdminMappers : Profile
    {
        public ShoolAdminMappers()
        {
            CreateMap<SchoolAdmin, SchoolAdminDTO>().ReverseMap();
            CreateMap<School, SchoolDTO>().ReverseMap();
            CreateMap<SchoolModerator, SchoolModeratorDTO>().ReverseMap();
            CreateMap<SchoolDTO, SchoolOnlyNameResponseApiModel>().ReverseMap();
            CreateMap<SchoolAdminDTO, SchoolAdminResponseApiModel>().ReverseMap();
        }
    }
}
