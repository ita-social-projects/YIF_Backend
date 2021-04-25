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
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;

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
        public async Task AddRangeOfSpecialtiesToIoE_ShouldReturnOk()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var chosen = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToInstitutionOfEducations.Remove(chosen);
            await _context.SaveChangesAsync();

            var paymentAndEducationForm = new PaymentAndEducationFormsResponseApiModel()
            {
                PaymentForm = YIF.Core.Data.Entities.PaymentForm.Contract,
                EducationForm = YIF.Core.Data.Entities.EducationForm.Daily
            };

            ICollection<PaymentAndEducationFormsResponseApiModel> collectionOfPaymentFormAndEducation = new PaymentAndEducationFormsResponseApiModel[] { paymentAndEducationForm };

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = chosen.SpecialtyId,
                InstitutionOfEducationId = chosen.InstitutionOfEducationId,
                PaymentAndEducationForms = collectionOfPaymentFormAndEducation
            };

            IEnumerable<SpecialtyToInstitutionOfEducationPostApiModel> collectionOfModels = new SpecialtyToInstitutionOfEducationPostApiModel[] { model };
            
            // Act            
            var response = await _client.PostAsync($"/api/InstitutionOfEducationAdmin/AddRangeSpecialtiesToInstitutionOfEducation", ContentHelper.GetStringContent(collectionOfModels));

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
    }
}