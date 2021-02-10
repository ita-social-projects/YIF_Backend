using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class UniversityAdminMappers: Profile
    {
        public UniversityAdminMappers()
        {
            CreateMap<UniversityAdmin, UniversityAdminDTO>().ReverseMap();
            CreateMap<UniversityAdminDTO, UniversityAdminResponseApiModel>().ReverseMap();
        }
    }
}
