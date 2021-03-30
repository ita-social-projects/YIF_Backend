using Moq;
using YIF.Core.Domain.ServiceInterfaces;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Unit.YIF_Backend.Controllers
{
    class InstitutionAdminControllerTest
    {
        private readonly Mock<IInstitutionAdminService> _institutionAdminService;
        private readonly InstitutionAdminController _institutionAdminController;

        public InstitutionAdminControllerTest()
        {
            _institutionAdminService = new Mock<IInstitutionAdminService>();
            _institutionAdminController = new InstitutionAdminController(_institutionAdminService.Object);
        }
    }
}