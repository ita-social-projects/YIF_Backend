using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.EntityForResponse;

namespace YIF.Core.Service.Mapping
{
    public class InstitutionOfEducationAdminMappers: Profile
    {
        public InstitutionOfEducationAdminMappers()
        {
            CreateMap<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>().ReverseMap();
            CreateMap<InstitutionOfEducationAdminDTO, InstitutionOfEducationAdminResponseApiModel>();
            CreateMap<InstitutionOfEducationDTO, IoEInfromationResponseApiModel>();

            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel>();

        }
    }
}
