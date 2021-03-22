using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISuperAdminService
    {
        /// <summary>
        /// Add admin if InstitutionOfEducation doesn`t have anyone.
        /// </summary>
        /// <param name="InstitutionOfEducationId"></param>
        /// <param name="AdminEmail">Admins email. If not exist to create</param>
        /// <param name="request"><see cref="HttpRequest"/> to reset password</param>
        /// <returns></returns>
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAdmin(
            string InstitutionOfEducationId,
            string AdminEmail,
            HttpRequest request);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> AddSchoolAdmin(SchoolAdminApiModel schoolAdminModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteInstitutionOfEducationAdmin(string id);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DisableInstitutionOfEducationAdmin(string id);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteSchoolAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAndAdmin(InstitutionOfEducationPostApiModel schoolUniAdminDeleteApi, HttpRequest request);
        Task<ResponseApiModel<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>> GetAllInstitutionOfEducationAdmins();
        Task<ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>> GetAllSchoolAdmins();
    }
}
