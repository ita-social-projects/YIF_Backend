using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Data.Entities;
using AutoMapper;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Service.Mapping
{
    public class UniversityAdminMappers: Profile
    {
        public UniversityAdminMappers()
        {
            CreateMap<UniversityAdmin, UniversityAdminDTO>()
                                                .ReverseMap();
        }
    }
}
