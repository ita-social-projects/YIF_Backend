using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class InstitutionOfEducationAdminControllerTests:TestServerFixture
    {
        private IoEAdminInputAttribute _adminInputAttribute;
        public InstitutionOfEducationAdminControllerTests(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _adminInputAttribute = new IoEAdminInputAttribute(_context);
        }

        [Fact]
        public async Task AddSpecialtyToIoE_ShouldReturnOk()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var chosen = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToInstitutionOfEducations.Remove(chosen);
            await _context.SaveChangesAsync();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = chosen.SpecialtyId,
                InstitutionOfEducationId = chosen.InstitutionOfEducationId
            };

            // Act            
            var response = await _client.PostAsync($"/api/InstitutionOfEducationAdmin/AddSpecialtyToInstitutionOfEducation", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteSpecialtyFromIoE_EndpointReturnNoContent()
        {
            //Arrange
            var institutionOfEducation = _context.InstitutionOfEducations.AsNoTracking().FirstOrDefault();
            var specialty = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().Where(x => x.Id == institutionOfEducation.Id).FirstOrDefault();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = specialty.SpecialtyId,
                InstitutionOfEducationId = institutionOfEducation.Id
            };

            //Act
            var response = await _client.PatchAsync(
                $"/api/Specialty/InstitutionOfEducationAdmin/DeleteSpecialtyFromInstitutionOfEducation", ContentHelper.GetStringContent(model));

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
