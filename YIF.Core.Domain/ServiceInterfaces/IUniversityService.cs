using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IUniversityService<T> where T : class
    {
        Task<ResponseApiModel<IEnumerable<UniversityFilterResponseApiModel>>> GetUniversityByFilter(FilterApiModel filterModel);
        Task<ResponseApiModel<UniversityResponseApiModel>> GetUniversityById(string filterModel);
        Task<UniversitiesPageResponseApiModel> GetUniversitiesPage(int page, int pageSize, string url);
    }
}
