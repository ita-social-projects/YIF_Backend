using AutoMapper;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class SpecialtyMapperProfile : Profile
    {
        public SpecialtyMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<Speciality, SpecialityDTO>().ReverseMap();
            CreateMap<SpecialityDTO, SpecialtyApiModel>()
                .ForMember(api => api.DirectionId, opt => opt.MapFrom(x => x.DirectionId))
                .ForMember(dto => dto.DirectionName, opt => opt.MapFrom<GetDirectionNameResolver>());
            CreateMap<SpecialtyApiModel, SpecialityDTO>()
                .ForMember(dto => dto.Direction, opt => opt.Ignore());
        }

        public class GetDirectionNameResolver : IValueResolver<SpecialityDTO, SpecialtyApiModel, string>
        {
            private static EFDbContext _context;
            public GetDirectionNameResolver(EFDbContext context)
            {
                _context = context;
            }
            public string Resolve(SpecialityDTO source, SpecialtyApiModel destination, string destMember, ResolutionContext context)
            {
                return _context.Directions.FirstOrDefault(x => x.Id == source.DirectionId).Name;
            }
        }

    }
}
