using AutoMapper;
using System.Linq;
using YIF.Core.Data;
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
            CreateMap<Speciality, SpecialityDTO>().ReverseMap();
            CreateMap<SpecialityDTO, SpecialtyResponseApiModel>();
            CreateMap<SpecialtyResponseApiModel, SpecialityDTO>()
                .ForMember(dto => dto.Direction, opt => opt.Ignore());
        }
    }
}
