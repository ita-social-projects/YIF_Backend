using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IIoEAdminService
    {
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddRangeSpecialtiesToIoE(IEnumerable<SpecialtyToInstitutionOfEducationPostApiModel> specialtyToIoE);
        Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyDescriptionOfInstitution(string userId, JsonPatchDocument<InstitutionOfEducationPostApiModel> institutionOfEducationPostApiModel);
        Task DeleteSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE);
        Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyDescription(SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel);
        Task<ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>> GetIoEModeratorsByUserId(string userId);
        Task<ResponseApiModel<IoEInformationResponseApiModel>> GetIoEInfoByUserId(string userId);
    }
}
