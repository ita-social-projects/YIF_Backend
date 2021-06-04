using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISuperAdminService
    {
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAdmin(
            string InstitutionOfEducationId,
            string AdminEmail,
            HttpRequest request);
        Task<ResponseApiModel<AuthenticateResponseApiModel>> AddSchoolAdmin(SchoolAdminApiModel schoolAdminModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteInstitutionOfEducationAdmin(string id);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteInstitutionOfEducation(string id);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DisableInstitutionOfEducationAdmin(string id);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteSchoolAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAndAdmin(InstitutionOfEducationCreatePostApiModel schoolUniAdminDeleteApi, HttpRequest request);
        Task<PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>> GetAllInstitutionOfEducationAdmins(InstitutionOfEducationAdminSortingModel institutionOfEducationAdminFilterModel, PageApiModel pageModel);
        Task<ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>> GetAllSchoolAdmins();
        Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyById(SpecialtyPutApiModel model);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToTheListOfAllSpecialties(SpecialtyPostApiModel specialtyPostApiModel);
        Task<ResponseApiModel<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>> GetIoEModeratorsByIoEId(string ioEId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> ChangeBannedStatusOfIoE(string IoEId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> ChooseIoEAdminFromModerators(IoEAdminAddFromModeratorsApiModel ioEAdminAddFromModeratorsApiModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> GetIoEAdminIdByIoEId(string ioEId);
    }
}
