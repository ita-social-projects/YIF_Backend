using System.Linq;
using System.Security.Claims;
using YIF.Core.Data;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    class UserInputAttribute
    {
        private EFDbContext _context;

        public UserInputAttribute(EFDbContext context)
        {
            _context = context;
        }

        public void SetUserIdForHttpContext()
        {
            var userId = _context.UserRoles.FirstOrDefault().UserId;

            FakePolicyEvaluator.claims = new[]
            {
                new Claim("id", userId)
            };
        }
    }
}
