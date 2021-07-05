using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ILectorService
    {
        Task<ResponseApiModel<IEnumerable<DepartmentApiModel>>> GetAllDepartments();
    }
}
