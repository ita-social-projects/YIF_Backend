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
            CreateMap<InstitutionOfEducationDTO, IoEInformationResponseApiModel>();

            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel>();
            CreateMap<InstitutionOfEducationAdminDTO, IoEAdminForIoEModeratorResponseApiModel>()
                .ForMember(sram => sram.Email, opt => opt.MapFrom(adm => adm.User.Email))
                .ForMember(sram => sram.Name, opt => opt.MapFrom(adm => adm.User.UserName));
        }
    }
}
