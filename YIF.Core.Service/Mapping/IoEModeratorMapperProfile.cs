using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class IoEModeratorMapperProfile : Profile
    {
        public IoEModeratorMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>().ReverseMap();

            CreateMap<InstitutionOfEducationModeratorDTO, IoEModeratorsResponseApiModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
