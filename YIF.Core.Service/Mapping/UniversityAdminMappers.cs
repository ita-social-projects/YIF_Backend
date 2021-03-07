using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.EntityForResponse;

namespace YIF.Core.Service.Mapping
{
    public class UniversityAdminMappers: Profile
    {
        public UniversityAdminMappers()
        {
            CreateMap<UniversityAdmin, UniversityAdminDTO>().ReverseMap();
            CreateMap<UniversityAdminDTO, UniversityAdminResponseApiModel>();

            CreateMap<UniversityDTO, UniversityForUniversityAdminResponseApiModel>();
        }
    }
}
