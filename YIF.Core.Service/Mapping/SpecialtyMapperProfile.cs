using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Service.Mapping
{
    class SpecialtyMapperProfile : Profile
    {
        public SpecialtyMapperProfile()
        {
            CreateMap<Speciality, SpecialtyDTO>().ReverseMap();
            //CreateMap<SpecialtyDTO, SpecialityApiModel>().ReverseMap();
        }
    }
}
