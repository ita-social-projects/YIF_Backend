﻿using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SuperAdminControllerTest : TestServerFixture
    {
        public SuperAdminControllerTest(ApiWebApplicationFactory fixture)
          : base(fixture)
        {
            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [Fact]
        public async Task AddInstitutionOfEducationAdmin_Output_Correct()
        {
            // Arrange
            var InstitutionOfEducation = new InstitutionOfEducation() { Name = "newInstitutionOfEducationTest1" };
            _context.InstitutionOfEducations.Add(InstitutionOfEducation);
            _context.SaveChanges();

            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = InstitutionOfEducation.Id, AdminEmail = "AdminEmailTest1@gmial.com" }
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(null, "test@gmail.com")]
        [InlineData("123", null)]
        [InlineData("", "test@gmail.com")]
        [InlineData("123", "")]
        public async Task AddInstitutionOfEducationAdmin_Input_WrongInstitutionOfEducationAdminApiModel(string institutionOfEducationId, string adminEmail)
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = institutionOfEducationId, AdminEmail = adminEmail }
            };
            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAdmin_Output_ByAddingSameAdminTwoTimes()
        {
            // Arrange
            var InstitutionOfEducation = new InstitutionOfEducation() { Name = "newInstitutionOfEducation" };
            _context.InstitutionOfEducations.Add(InstitutionOfEducation);
            _context.SaveChanges();
            
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = InstitutionOfEducation.Id, AdminEmail = "AdminEmailTest@gmial.com" }
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.AdminEmail = "AdminEmailTest2@gmial.com";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Theory]
        [MemberData(nameof(SuperAdminInputAttribute.GetWrongData), MemberType = typeof(SuperAdminInputAttribute))]
        public async Task AddInstitutionOfEducationAndAdmin_Input_WrongInstitutionOfEducationPostApiModel_site(StringContent content)
        {
            // Act
            var response = await _client.PostAsync("/api/SuperAdmin/AddInstitutionOfEducationAndAdmin", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_WithCorectData()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_ByAddingSameInstitutionOfEducationTwoTimes()
        {            
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.InstitutionOfEducationAdminEmail = "name@gmail.com";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_ByAddingSameAdminTwoTimes()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.Name = "name";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task GetAllInstitutionOfEducations()
        {
            // Arrange
            var request = "/api/SuperAdmin/GetAllInstitutionOfEducations";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteInstitutionOfEducationAdmin()
        {
            // Arrange
            var admin = _context.InstitutionOfEducationAdmins.First();
            
            // Act
            var response = await _client.DeleteAsync(string.Format("/api/SuperAdmin/DeleteInstitutionOfEducationAdmin/{0}", admin.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DisableInstitutionOfEducationAdmin()
        {
            // Arrange
            var admin = _context.InstitutionOfEducationAdmins.First();

            // Act
            var response = await _client.PostAsync(string.Format("/api/SuperAdmin/DisableInstitutionOfEducationAdmin/{0}", admin.Id), ContentHelper.GetStringContent(admin));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}