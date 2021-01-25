using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class GraduateMapperProfile : Profile
    {
        public GraduateMapperProfile()
        {
            CreateMap<Graduate, GraduateDTO>().ReverseMap();
        }
    }
}
