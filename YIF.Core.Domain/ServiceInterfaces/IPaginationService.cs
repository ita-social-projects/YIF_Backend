using System.Collections.Generic;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IPaginationService
    {
        PageResponseApiModel<T> GetPageFromCollection<T>(IEnumerable<T> list, int pageNumber, int pageSize, string url = null);
    }
}
