using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISuperAdminService
    {
        Task<ResponseApiModel<AuthenticateResponseApiModel>> AddUniversityAdmin(UniversityAdminApiModel universityAdminModel);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> AddSchoolAdmin(SchoolAdminApiModel schoolAdminModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteUniversityAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteSchoolAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddUniversity(UniversityPostApiModel schoolUniAdminDeleteApi);
        //Task<ResponseApiModel<DescriptionResponseApiModel>> RestoreAdmin(RestoreSuperAdminApiModel restoreSuperAdminApiModel);
        Task<ResponseApiModel<IEnumerable<UniversityAdminResponseApiModel>>> GetAllUniversityAdmins();
        Task<ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>> GetAllSchoolAdmins();
    }
}
