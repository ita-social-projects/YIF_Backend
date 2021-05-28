using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class DirectionMapperProfile : Profile
    {
        public DirectionMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<DirectionToInstitutionOfEducationDTO, DirectionToIoEResponseApiModel>()
                .ForMember(dti => dti.Id, opt => opt.MapFrom(src => src.Direction.Id))
                .ForMember(dti => dti.Name, opt => opt.MapFrom(src => src.Direction.Name))
                .ForMember(dti => dti.Code, opt => opt.MapFrom(src => src.Direction.Code));
            CreateMap<SpecialtyDTO, SpecialtyForDirectionResponseModel>();
            CreateMap<Direction, DirectionDTO>().ReverseMap();
            CreateMap<DirectionDTO, DirectionResponseApiModel>().ReverseMap();
            CreateMap<DirectionPostApiModel, DirectionDTO>();
        }
    }
}
