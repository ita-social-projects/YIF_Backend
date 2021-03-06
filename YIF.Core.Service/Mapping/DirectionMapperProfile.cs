using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class DirectionMapperProfile : Profile
    {
        public DirectionMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<SpecialtyDTO, SpecialtyForDirectionResponseModel>()
                .ForMember(dst => dst.SpecialtyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SpecialtyCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.SpecialtyName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Direction, DirectionDTO>().ReverseMap();
            CreateMap<DirectionDTO, DirectionResponseApiModel>().ReverseMap();
          
        }
    }
}
