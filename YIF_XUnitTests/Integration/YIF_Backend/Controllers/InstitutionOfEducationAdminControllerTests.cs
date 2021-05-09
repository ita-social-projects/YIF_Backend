using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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
    public class InstitutionOfEducationAdminControllerTests : TestServerFixture
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
            var specialty = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = specialty.SpecialtyId,
                InstitutionOfEducationId = specialty.InstitutionOfEducationId
            };

            //Act
            var response = await _client.PatchAsync(
                $"/api/InstitutionOfEducationAdmin/DeleteSpecialtyFromInstitutionOfEducation", ContentHelper.GetStringContent(model));

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateSpecialtyDescription_EndpointReturnOk()
        {
            //Arrange
            var description = _context.SpecialtyToIoEDescriptions.Where(x => x.Description != null).AsNoTracking().FirstOrDefault();
            var examRequirements = _context.ExamRequirements.AsNoTracking().Where(x => x.SpecialtyToIoEDescriptionId == description.Id).ToList();

            var examRequirementsUpdateApiModel = new List<ExamRequirementUpdateApiModel>();
            foreach (var item in examRequirements)
            {
                examRequirementsUpdateApiModel.Add(new ExamRequirementUpdateApiModel
                {
                    ExamId = item.ExamId,
                    SpecialtyToIoEDescriptionId = item.SpecialtyToIoEDescriptionId,
                    Coefficient = item.Coefficient,
                    MinimumScore = item.MinimumScore
                });
            }

            var model = new SpecialtyDescriptionUpdateApiModel
            {
                Id = description.Id,
                SpecialtyToInstitutionOfEducationId = description.SpecialtyToInstitutionOfEducationId,
                PaymentForm = description.PaymentForm,
                EducationForm = description.EducationForm,
                EducationalProgramLink = description.EducationalProgramLink,
                Description = description.Description,
                ExamRequirements = examRequirementsUpdateApiModel
            };

            // Act            
            var response = await _client.PutAsync($"/api/InstitutionOfEducationAdmin/Specialty/Description/Update", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetModerators_EndpointReturnsListOfModeratorsWithOkStatusCode_IfEverythingIsOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();

            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducationAdmin/GetIoEModerators");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetIoEInfo_EndpointReturnsIoEWithOkStatusCode_IfEverythingIsOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();

            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducationAdmin/GetIoEInfoByUserId");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetSpecialtyDescription_EndpointReturnsSuccessAndCorrectContentType()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var admin = _context.InstitutionOfEducationAdmins.AsNoTracking().FirstOrDefault();
            var institutionOfEducation = _context.InstitutionOfEducations.AsNoTracking().Where(i => i.Id == admin.InstitutionOfEducationId).FirstOrDefault();
            var specialtyToIoE = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().Where(x => x.InstitutionOfEducationId == institutionOfEducation.Id).FirstOrDefault();

            //Act
            var response = await _client.GetAsync(
                $"/api/InstitutionOfEducationAdmin/Specialty/Description/Get/" + specialtyToIoE.SpecialtyId);

            //Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8",
                 response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task BanIoEModerator_ShouldReturnOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var ioEModerator = _context.InstitutionOfEducationModerators.AsNoTracking().FirstOrDefault();

            // Act
            var response = await _client.PatchAsync(string.Format("/api/InstitutionOfEducationAdmin/BanIoEModerator/{0}",
                ioEModerator.UserId), ContentHelper.GetStringContent(ioEModerator));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}