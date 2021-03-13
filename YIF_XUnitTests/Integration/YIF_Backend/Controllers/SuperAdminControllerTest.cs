﻿using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
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

        [Theory]
        [MemberData(nameof(SuperAdminInputAttribute.GetWrongData), MemberType = typeof(SuperAdminInputAttribute))]
        public async Task AddInstitutionOfEducationAndAdmin_Input_WrongInstitutionOfEducationPostApiModel_site(StringContent content)
        {
            // Act
            var response = await _client.PostAsync("/api/SuperAdmin/AddInstitutionOfEducationAndAdmin", content);

            // Assert
            Assert.True( response.StatusCode == System.Net.HttpStatusCode.BadRequest);
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
    }
}