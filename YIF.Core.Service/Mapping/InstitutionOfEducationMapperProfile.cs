using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class InstitutionOfEducationMapperProfile : Profile
    {
        public InstitutionOfEducationMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<InstitutionOfEducation, InstitutionOfEducationDTO>().ReverseMap();
            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationResponseApiModel>();
            CreateMap<InstitutionOfEducationDTO, InstitutionsOfEducationResponseApiModel>();

            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationPostApiModel>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore())
                .ReverseMap();

            CreateMap<InstitutionOfEducationDTO, InstitutionOfEducationCreatePostApiModel>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore())
                .ForMember(post => post.InstitutionOfEducationAdminEmail, un => un.Ignore())
                .ReverseMap();

            CreateMap<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>()
                .ReverseMap();

            CreateMap<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>()
                .ReverseMap();

            CreateMap<JsonPatchDocument<InstitutionOfEducationPostApiModel>, JsonPatchDocument<InstitutionOfEducationDTO>>();
            CreateMap<Operation<InstitutionOfEducationPostApiModel>, Operation<InstitutionOfEducationDTO>>();

            CreateMap<Direction, DirectionDTO>()
                .ReverseMap();
            CreateMap<DirectionDTO, DirectionForIoEResponseApiModel>();

            CreateMap<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>()
                .ReverseMap();

            CreateMap<Specialty, SpecialtyDTO>()
                .ReverseMap();
            CreateMap<SpecialtyDTO, SpecialtyForDirectionResponseModel>();

            CreateMap<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>()
                .ReverseMap();

            CreateMap<Lecture, LectureDTO>()
                .ReverseMap();
        }
    }
}
