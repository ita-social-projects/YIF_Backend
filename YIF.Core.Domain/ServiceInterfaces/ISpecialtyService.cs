using System;
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
        Task<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>> GetAllSpecialtyDescriptionsById(string id);
        Task AddSpecialtyAndInstitutionOfEducationToFavorite(string specialtyId, string institutionOfEducationId, string userId);
        Task DeleteSpecialtyAndInstitutionOfEducationFromFavorite(string specialtyId, string institutionOfEducationId, string userId);
        Task AddSpecialtyToFavorite(string specialtyId, string userId);
        Task DeleteSpecialtyFromFavorite(string specialtyId, string userId);
        Task DeleteSpecialtyFromInstitutionOfEducation(string specialtyId, string institutionOfEducationId);
    }
}
