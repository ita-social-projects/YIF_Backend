using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class LectorMapper : Profile
    {
        public LectorMapper()
        {
            AllowNullCollections = true;

            CreateMap<Lector, LectorDTO>().ReverseMap();

            CreateMap<LectorDTO, LectorResponseApiModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dst => dst.IoEId, opt => opt.MapFrom(src => src.InstitutionOfEducationId))
                .ForMember(dst => dst.LectorId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
