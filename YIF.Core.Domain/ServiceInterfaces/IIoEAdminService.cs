using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IIoEAdminService
    {
        Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyInstitution(string userId, JsonPatchDocument<InstitutionOfEducationPostApiModel> institutionOfEducationPostApiModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE);
        Task DeleteSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE);
        Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyDescription(SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel);
        Task<ResponseApiModel<SpecialtyToInstitutionOfEducationResponseApiModel>> GetSpecialtyToIoEDescription(string userId, string specialtyId);
        Task<ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>> GetIoEModeratorsByUserId(string userId);
        Task<ResponseApiModel<IoEInformationResponseApiModel>> GetIoEInfoByUserId(string userId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> ChangeBannedStatusOfIoEModerator(string moderatorId, string userId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteIoEModerator(string moderatorId, string userId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddIoEModerator(string moderatorEmail, string userId, HttpRequest request);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddLectorToIoE(string userId, EmailApiModel email, HttpRequest request);
        Task<ResponseApiModel<IEnumerable<LectorResponseApiModel>>> GetIoELectorsByUserId(string userId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteIoELector(string lectorId, string userId);
        Task<ResponseApiModel<DescriptionResponseApiModel>> RestoreIoEModerator(string moderatorId, string userId);
    }
}

