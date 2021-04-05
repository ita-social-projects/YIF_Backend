using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class InstitutionOfEducationAdminResponseApiModel
    {
        /// <summary>
        /// Institution of education Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public UserForInstitutionOfEducationAdminResponseApiModel User { get; set; }

        /// <summary>
        /// Institution of education
        /// </summary>
        public InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel InstitutionOfEducation { get; set; }

        /// <summary>
        /// Status of banning
        /// </summary>
        public bool IsBanned { get; set; }
    }
}
