using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IInstitutionOfEducationService<T> where T : class
    {
        Task<IEnumerable<InstitutionOfEducationResponseApiModel>> GetInstitutionOfEducationsByFilter(FilterApiModel filterModel);
        Task<InstitutionOfEducationResponseApiModel> GetInstitutionOfEducationById(string institutionOfEducationId, string userId = null);
        Task<PageResponseApiModel<InstitutionOfEducationResponseApiModel>> GetInstitutionOfEducationsPage(FilterApiModel filterModel, PageApiModel pageModel);
        Task<PageResponseApiModel<InstitutionOfEducationResponseApiModel>> GetInstitutionOfEducationsPageForUser(FilterApiModel filterModel, PageApiModel pageModel, string userId);
        Task<IEnumerable<InstitutionOfEducationResponseApiModel>> GetFavoriteInstitutionOfEducations(string userId);
        Task<IEnumerable<string>> GetInstitutionOfEducationAbbreviations(FilterApiModel filterModel);
        Task AddInstitutionOfEducationToFavorite(string institutionOfEducationId, string userId);
        Task DeleteInstitutionOfEducationFromFavorite(string institutionOfEducationId, string userId);
    }
}