using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class IoEBufferMapperProfile : Profile
    {
        public IoEBufferMapperProfile()
        {
            CreateMap<IoEBufferDTO, InstitutionOfEducationPostApiModel>()
                .ReverseMap();

            CreateMap<InstitutionOfEducationDTO, IoEBufferDTO>();

            CreateMap<IoEBuffer, IoEBufferDTO>()
                .ReverseMap();

            CreateMap<InstitutionOfEducationDTO, IoEChangesForSuperAdminResponceApiModel>();

            CreateMap<IoEBufferDTO, IoEChangesForSuperAdminResponceApiModel>();

            CreateMap<IoEBufferDTO, InstitutionOfEducationPostApiModel>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore());

            CreateMap<InstitutionOfEducation, IoEBufferDTO>().ReverseMap();
        }
    }
}
