using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IIoEAdminService
    {
        Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyDescriptionOfInstitution(string userId, InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel);
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE);
        Task DeleteSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE);
    }
}
