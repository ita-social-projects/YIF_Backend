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

            CreateMap<InstitutionOfEducationModeratorDTO, IoEModeratorsForSuperAdminResponseApiModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<InstitutionOfEducationModeratorDTO, IoEModeratorsForIoEAdminResponseApiModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dst => dst.ModeratorId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
