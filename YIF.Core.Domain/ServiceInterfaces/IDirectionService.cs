using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IDirectionService
    {
        Task<PageResponseApiModel<DirectionResponseApiModel>> GetAllDirections(int page, int pageSize, string url);
        Task<IEnumerable<string>> GetDirectionNames();
    }
}
