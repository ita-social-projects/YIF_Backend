using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUniversityService<T> where T : class
    {
        Task<IEnumerable<UniversityResponseApiModel>> GetUniversitiesByFilter(FilterApiModel filterModel);
        Task<UniversityResponseApiModel> GetUniversityById(string universityId, string userId = null);
        Task<PageResponseApiModel<UniversityResponseApiModel>> GetUniversitiesPage(FilterApiModel filterModel, PageApiModel pageModel, string userId = null);
        Task<IEnumerable<UniversityResponseApiModel>> GetFavoriteUniversities(string userId);
        Task<IEnumerable<string>> GetUniversityAbbreviations(FilterApiModel filterModel);

    }
}
