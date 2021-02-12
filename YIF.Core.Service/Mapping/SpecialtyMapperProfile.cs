using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class SpecialtyMapperProfile : Profile
    {
        public SpecialtyMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<Specialty, SpecialtyDTO>().ReverseMap();
            CreateMap<SpecialtyDTO, SpecialtyResponseApiModel>();
            CreateMap<SpecialtyResponseApiModel, SpecialtyDTO>()
                .ForMember(dto => dto.Direction, opt => opt.Ignore());
        }
    }
}
