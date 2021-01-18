using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class UniversityMappers : Profile
    {
        public UniversityMappers()
        {
            CreateMap<University, UniversityDTO>().ReverseMap();
            CreateMap<University, UniversityPostApiModel>().ReverseMap();
        }
       
    }
}
