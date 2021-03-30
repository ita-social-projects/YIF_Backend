using System.Linq;
using System.Security.Claims;
using YIF.Core.Data;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    public class IoEModeratorInputAttribute
    {
        private EFDbContext _context;

        public IoEModeratorInputAttribute(EFDbContext context)
        {
            _context = context;
        }

        public void SetUserIdByIoEModeratorUserIdForHttpContext()
        {
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == "InstitutionOfEducationModerator").Id;
            var userId = _context.UserRoles.FirstOrDefault(x => x.RoleId == roleId).UserId;

            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }
    }
}
