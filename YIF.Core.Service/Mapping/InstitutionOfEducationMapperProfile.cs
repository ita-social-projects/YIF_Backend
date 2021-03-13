using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class InstitutionOfEducationMapperProfile : Profile
    {
        public InstitutionOfEducationMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<InstitutionOfEducation, InstitutionOfEducationDTO>().ReverseMap();
            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationResponseApiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));

            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationPostApiModel>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore())
                .ForMember(post => post.InstitutionOfEducationAdminEmail, un => un.Ignore())
                .ReverseMap();

            CreateMap<Direction, DirectionDTO>()
                .ReverseMap();
            CreateMap<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>()
                .ReverseMap();

            CreateMap<Specialty, SpecialtyDTO>()
                .ReverseMap();
            CreateMap<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>()
                .ReverseMap();

            CreateMap<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>()
                .ReverseMap();
            CreateMap<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>()
                .ReverseMap();
            CreateMap<Lecture, LectureDTO>()
                .ReverseMap();
        }
    }
}
