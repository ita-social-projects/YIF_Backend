using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class UniversityMapperProfile : Profile
    {
        public UniversityMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<University, UniversityDTO>().ReverseMap();
            CreateMap<UniversityDTO, UniversityResponseApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));

            CreateMap<UniversityDTO, UniversityPostApiModel>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore())
                .ForMember(post => post.UniversityAdminEmail, un => un.Ignore())
                .ReverseMap();

            CreateMap<Direction, DirectionDTO>()
                .ReverseMap();
            CreateMap<DirectionToUniversity, DirectionToUniversityDTO>()
                .ReverseMap();

            CreateMap<Specialty, SpecialtyDTO>()
                .ReverseMap();
            CreateMap<SpecialtyToUniversity, SpecialtyToUniversityDTO>()
                .ReverseMap();

            CreateMap<UniversityAdmin, UniversityAdminDTO>()
                .ReverseMap();
            CreateMap<UniversityModerator, UniversityModeratorDTO>()
                .ReverseMap();
            CreateMap<Lecture, LectureDTO>()
                .ReverseMap();
        }
    }
}
