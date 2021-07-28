using System.Linq;
using System.Security.Claims;
using YIF.Core.Data;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    public class LectorInputAttribute
    {
        private EFDbContext _context;

        public LectorInputAttribute(EFDbContext context)
        {
            _context = context;
        }

        public void SetUserIdByLectorUserIdForHttpContext()
        {
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == "Lector").Id;
            var userId = _context.UserRoles.FirstOrDefault(x => x.RoleId == roleId).UserId;

            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }

        public void SetUserIdByLectorUserIdForHttpContext(string userId)
        {
            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }
    }
}
