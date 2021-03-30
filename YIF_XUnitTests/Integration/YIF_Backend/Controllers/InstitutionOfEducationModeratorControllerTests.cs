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
    public class InstitutionOfEducationModeratorControllerTests:TestServerFixture
    {
        private IoEModeratorInputAttribute _moderatorInputAttribute;
        public InstitutionOfEducationModeratorControllerTests(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _moderatorInputAttribute = new IoEModeratorInputAttribute(_context);
        }

        [Fact]
        public async Task AddSpecialtyToIoE_ShouldReturnOk()
        {
            //Arrange
            _moderatorInputAttribute.SetUserIdByIoEModeratorUserIdForHttpContext();
            var chosen = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToInstitutionOfEducations.Remove(chosen);
            await _context.SaveChangesAsync();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = chosen.SpecialtyId,
                InstitutionOfEducationId = chosen.InstitutionOfEducationId
            };

            // Act            
            var response = await _client.PostAsync($"/api/InstitutionOfEducationModerator/AddSpecialtyToInstitutionOfEducation", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
