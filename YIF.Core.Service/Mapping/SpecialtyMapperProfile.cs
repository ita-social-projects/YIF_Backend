using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
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
            CreateMap<SpecialtyPutApiModel, SpecialtyDTO>();
            CreateMap<SpecialtyDTO, SpecialtyResponseApiModel>();
            CreateMap<SpecialtyResponseApiModel, SpecialtyDTO>()
                .ForMember(dto => dto.Direction, opt => opt.Ignore());

            CreateMap<Exam, ExamDTO>().ReverseMap();

            CreateMap<ExamDTO, ExamResponseApiModel>();
            CreateMap<ExamDTO, ExamsResponseApiModel>();

            CreateMap<ExamRequirement, ExamRequirementDTO>().ReverseMap();

            CreateMap<ExamRequirementDTO, ExamRequirementsResponseApiModel>()
                .ForMember(dst => dst.ExamName, opt => opt.MapFrom(src => src.Exam.Name));

            CreateMap<ExamRequirementUpdateApiModel, ExamRequirementDTO>();

            CreateMap<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>().ReverseMap();
            CreateMap<SpecialtyToIoEDescriptionDTO, SpecialtyToIoEDescriptionResponseApiModel>();

            CreateMap<SpecialtyDescriptionUpdateApiModel, SpecialtyToIoEDescriptionDTO>();

            CreateMap<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>().ReverseMap();
            CreateMap<SpecialtyToInstitutionOfEducationPostApiModel, SpecialtyToInstitutionOfEducationDTO>();

            CreateMap<SpecialtyToInstitutionOfEducationDTO, SpecialtyToInstitutionOfEducationResponseApiModel>()
                .ForMember(dst => dst.InstitutionOfEducationAbbreviation, opt => opt.MapFrom(src => src.InstitutionOfEducation.Abbreviation))
                .ForMember(dst => dst.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name))
                .ForMember(dst => dst.SpecialtyCode, opt => opt.MapFrom(src => src.Specialty.Code))
                .ForMember(dst => dst.Descriptions, opt => opt.MapFrom(src => src.SpecialtyToIoEDescriptions));

            CreateMap<SpecialityPostApiModel, SpecialtyDTO>();
            CreateMap<SpecialtyToInstitutionOfEducationToGraduate, SpecialtyToInstitutionOfEducationToGraduateDTO>().ReverseMap();
            CreateMap<SpecialtyAndInstitutionOfEducationToFavoritePostApiModel, SpecialtyToInstitutionOfEducationToGraduateDTO>();
            CreateMap<SpecialtyToInstitutionOfEducationPostApiModel, SpecialtyToInstitutionOfEducationToGraduateDTO>();
        }
    }
}
