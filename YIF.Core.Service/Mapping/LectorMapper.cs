using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class LectorMapper : Profile
    {
        public LectorMapper()
        {
            AllowNullCollections = true;

            CreateMap<Lector, LectorDTO>()
                .ForMember(post => post.ImageApiModel, un => un.Ignore())
                .ReverseMap();

            CreateMap<LectorDTO, LectorResponseApiModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dst => dst.IoEId, opt => opt.MapFrom(src => src.InstitutionOfEducationId))
                .ForMember(dst => dst.LectorId, opt => opt.MapFrom(src => src.Id));

            CreateMap<JsonPatchDocument<LectorApiModel>, JsonPatchDocument<LectorDTO>>();
            CreateMap<Operation<LectorApiModel>, Operation<LectorDTO>>();

            CreateMap<LectorDTO, LectorApiModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(lec => lec.User.UserName))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(lec => lec.User.Email))
                .ReverseMap();
        }
    }
}
