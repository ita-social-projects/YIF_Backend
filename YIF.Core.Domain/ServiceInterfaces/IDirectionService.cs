using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IDirectionService
    {
        Task<PageResponseApiModel<DirectionResponseApiModel>> GetAllDirections(PageApiModel pageModel);
        Task<IEnumerable<string>> GetDirectionsNamesByFilter(FilterApiModel filterModel);
        Task<IEnumerable<DirectionResponseApiModel>> GetAllDirectionsByFilter(FilterApiModel filterModel);
    }
}
