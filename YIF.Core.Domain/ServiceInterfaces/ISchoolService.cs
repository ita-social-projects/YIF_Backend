using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISchoolService
    {
        Task<ResponseApiModel<IEnumerable<SchoolOnlyNameResponseApiModel>>> GetAllSchoolNames();
        Task<ResponseApiModel<IEnumerable<string>>> GetAllSchoolNamesAsStrings();
    }
}
