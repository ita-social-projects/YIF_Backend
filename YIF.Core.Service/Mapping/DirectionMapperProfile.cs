using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    class DirectionMapperProfile : Profile
    {
        public DirectionMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<Direction, DirectionDTO>().ReverseMap();
        }
    }
}
