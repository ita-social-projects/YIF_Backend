using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IInstitutionAdminService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">Admin Id to define institution</param>
        /// <param name="institutionOfEducationPostApiModel">All params for institution</param>
        /// <returns></returns>
        Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyDescriptionOfInstitution(string userId, InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel);
    }
}
