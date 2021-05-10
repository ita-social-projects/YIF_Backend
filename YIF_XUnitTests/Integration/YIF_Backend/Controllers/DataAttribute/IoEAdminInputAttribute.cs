using System.Linq;
using System.Security.Claims;
using YIF.Core.Data;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    public class IoEAdminInputAttribute
    {
        private EFDbContext _context;

        public IoEAdminInputAttribute(EFDbContext context)
        {
            _context = context;
        }

        public void SetUserIdByIoEAdminUserIdForHttpContext()
        {
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == "InstitutionOfEducationAdmin").Id;
            var userId = _context.UserRoles.FirstOrDefault(x => x.RoleId == roleId).UserId;

            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }

        public void SetUserIdByIoEAdminUserIdForHttpContext(string userId)
        {
            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }
    }
}
