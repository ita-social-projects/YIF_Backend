using System.Linq;
using System.Security.Claims;
using YIF.Core.Data;
using YIF_XUnitTests.Integration.Fixture;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute
{
    public class SpecialtyInputAttribute
    {
        private EFDbContext _context;
        public SpecialtyInputAttribute(EFDbContext context)
        {
            _context = context;
        }
        public void SetUserIdByGraduateUserIdForHttpContext()
        {
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == "Graduate").Id;
            var userId = _context.UserRoles.FirstOrDefault(x => x.RoleId == roleId).UserId;

            FakePolicyEvaluator.claims = new[] {
            new Claim("id", userId) };
        }
        public void SetUserIdForHttpContext(string Id)
        {
            FakePolicyEvaluator.claims = new[] {
            new Claim("id", Id) };
        }
    }
}
