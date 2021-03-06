﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISpecialtyService : IDisposable
    {
        Task<ResponseApiModel<SpecialtyResponseApiModel>> GetSpecialtyById(string id);
        Task<ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>> GetAllSpecialties();
        Task<IEnumerable<SpecialtyResponseApiModel>> GetAllSpecialtiesByFilter(FilterApiModel filterModel);
        Task<IEnumerable<string>> GetSpecialtiesNamesByFilter(FilterApiModel filterModel);
        Task<IEnumerable<SpecialtyToUniversityResponseApiModel>> GetAllSpecialtyDescriptionsById(string id);
        Task AddSpecialtyAndUniversityToFavorite(string specialtyId, string universityId, string userId);
        Task DeleteSpecialtyAndUniversityFromFavorite(string specialtyId, string universityId, string userId);

    }
}
