using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using AutoMapper;

namespace YIF.Core.Service.Mapping
{
    class DisciplineMapperProfile: Profile
    {
        public DisciplineMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<Discipline, DisciplineDTO>().ReverseMap();
            CreateMap<DisciplinePostApiModel, Discipline>();
        }
    }
}
