using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversityAdminResponseApiModel
    {
        public string Id { get; set; }
        public UserForUniversityAdminResponseApiModel User { get; set; }
        public UniversityForUniversityAdminResponseApiModel University { get; set; }
        public bool IsBanned { get; set; }
    }
}
