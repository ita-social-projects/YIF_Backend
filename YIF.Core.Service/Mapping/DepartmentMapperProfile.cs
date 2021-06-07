using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class DepartmentMapperProfile : Profile
    {
        public DepartmentMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
