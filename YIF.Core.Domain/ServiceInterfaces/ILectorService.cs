using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using System.Collections.Generic;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ILectorService
    {
        public Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyLector(string userId, JsonPatchDocument<LectorApiModel> institutionOfEducationPostApiModel);
        Task<ResponseApiModel<IEnumerable<DepartmentApiModel>>> GetAllDepartments();
    }
}
