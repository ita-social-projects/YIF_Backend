using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class InstitutionOfEducationAdminResponseApiModel
    {
        public string Id { get; set; }
        public UserForInstitutionOfEducationAdminResponseApiModel User { get; set; }
        public InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel InstitutionOfEducation { get; set; }
        public bool IsBanned { get; set; }
    }
}
