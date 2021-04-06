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
            CreateMap<SpecialtyDTO, SpecialtyResponseApiModel>();
            CreateMap<SpecialtyResponseApiModel, SpecialtyDTO>()
                .ForMember(dto => dto.Direction, opt => opt.Ignore());

            CreateMap<Exam, ExamDTO>();
            CreateMap<EducationForm, EducationFormDTO>();
            CreateMap<PaymentForm, PaymentFormDTO>();

            CreateMap<ExamDTO, ExamResponseApiModel>();
            CreateMap<EducationFormDTO, EducationFormResponseApiModel>();
            CreateMap<PaymentFormDTO, PaymentFormResponseApiModel>();

            CreateMap<ExamRequirement, ExamRequirementDTO>();
            CreateMap<EducationFormToDescription, EducationFormToDescriptionDTO>();
            CreateMap<PaymentFormToDescription, PaymentFormToDescriptionDTO>();

            CreateMap<ExamRequirementDTO, ExamRequirementsResponseApiModel>()
                .ForMember(dst => dst.ExamName, opt => opt.MapFrom(src => src.Exam.Name));
            CreateMap<EducationFormToDescriptionDTO, EducationFormToDescriptionResponseApiModel>()
                .ForMember(dst => dst.EducationFormName, opt => opt.MapFrom(src => src.EducationForm.Name));
            CreateMap<PaymentFormToDescriptionDTO, PaymentFormToDescriptionResponseApiModel>()
                .ForMember(dst => dst.PaymentFormName, opt => opt.MapFrom(src => src.PaymentForm.Name));

            CreateMap<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>();
            CreateMap<SpecialtyToIoEDescriptionDTO, SpecialtyToIoEDescriptionResponseApiModel>();

            CreateMap<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>().ReverseMap();
            CreateMap<SpecialtyToInstitutionOfEducationPostApiModel, SpecialtyToInstitutionOfEducationDTO>();

            CreateMap<SpecialtyToInstitutionOfEducationDTO, SpecialtyToInstitutionOfEducationResponseApiModel>()
                .ForMember(dst => dst.InstitutionOfEducationAbbreviation, opt => opt.MapFrom(src => src.InstitutionOfEducation.Abbreviation))
                .ForMember(dst => dst.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name))
                .ForMember(dst => dst.SpecialtyCode, opt => opt.MapFrom(src => src.Specialty.Code))
                .ForMember(dst => dst.EducationalProgramLink, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.EducationalProgramLink))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.Description))
                .ForMember(dst => dst.ExamRequirements, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.ExamRequirements))
                .ForMember(dst => dst.EducationFormToDescriptions, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.EducationFormToDescriptions))
                .ForMember(dst => dst.PaymentFormToDescriptions, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.PaymentFormToDescriptions));

            CreateMap<SpecialityPostApiModel, SpecialtyDTO>();
        }
    }
}
