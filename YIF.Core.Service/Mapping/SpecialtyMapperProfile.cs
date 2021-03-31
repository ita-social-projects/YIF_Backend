using AutoMapper;
using System;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;
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

            CreateMap<Exam, ExamDTO>().ReverseMap();

            CreateMap<ExamDTO, ExamResponseApiModel>();
            CreateMap<ExamDTO, ExamsResponseApiModel>();

            CreateMap<ExamRequirement, ExamRequirementDTO>().ReverseMap();

            CreateMap<ExamRequirementDTO, ExamRequirementsResponseApiModel>()
                .ForMember(dst => dst.ExamName, opt => opt.MapFrom(src => src.Exam.Name));

            CreateMap<ExamRequirementDTO, ExamRequirementForEditPageResponseApiModel>()
                .ForMember(dst => dst.ExamId, opt => opt.MapFrom(scr => scr.ExamId))
                .ForMember(dst => dst.ExamName, opt => opt.MapFrom(src => src.Exam.Name));

            CreateMap<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>().ReverseMap();
            CreateMap<SpecialtyToIoEDescriptionDTO, SpecialtyToIoEDescriptionResponseApiModel>()
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.EducationalProgramLink, opt => opt.MapFrom(src => src.EducationalProgramLink))
                .ForMember(dst => dst.EducationForm, opt => opt.MapFrom(src => src.EducationForm))
                .ForMember(dst => dst.PaymentForm, opt => opt.MapFrom(src => src.PaymentForm))
                .ForMember(dst => dst.ExamRequirements, opt => opt.MapFrom(src => src.ExamRequirements));

            CreateMap<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>().ReverseMap();
            CreateMap<SpecialtyToInstitutionOfEducationPostApiModel, SpecialtyToInstitutionOfEducationDTO>();

            CreateMap<SpecialtyToInstitutionOfEducationDTO, SpecialtyToInstitutionOfEducationResponseApiModel>()
                .ForMember(dst => dst.InstitutionOfEducationAbbreviation, opt => opt.MapFrom(src => src.InstitutionOfEducation.Abbreviation))
                .ForMember(dst => dst.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name))
                .ForMember(dst => dst.SpecialtyCode, opt => opt.MapFrom(src => src.Specialty.Code))
                .ForMember(dst => dst.Descriptions, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription));

            CreateMap<SpecialtyToInstitutionOfEducationDTO, SpecialtyDescriptionForEditPageResponseApiModel>()
                .ForMember(dst => dst.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name))
                .ForMember(dst => dst.SpecialtyCode, opt => opt.MapFrom(src => src.Specialty.Code))
                .ForMember(dst => dst.EducationalProgramLink, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.EducationalProgramLink))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.Description))
                .ForMember(dst => dst.ExamRequirements, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.ExamRequirements))
                .ForMember(dst => dst.EducationForm, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.EducationForm))
                .ForMember(dst => dst.PaymentForm, opt => opt.MapFrom(src => src.SpecialtyToIoEDescription.PaymentForm));
        }
    }
}
