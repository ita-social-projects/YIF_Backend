using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_Backend;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SpecialtyControllerTests: TestServerFixture
    {
        private readonly SpecialtyInputAttribute _specialtyInputAttribute;
        public SpecialtyControllerTests(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _specialtyInputAttribute = new SpecialtyInputAttribute(_context);
        }

        [Fact]
        public async Task GetAll_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act            
            var response = await _client.GetAsync("/api/Specialty/All");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetNames_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act            
            var response = await _client.GetAsync("/api/Specialty/Names");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }


        [Theory]
        [InlineData("00f5e3b3-fa90-4e74-856f-000000000000")]
        public async Task GetById_EndpointReturnNotFound(string url)
        {
            // Act            
            var response = await _client.GetAsync($"/api/Specialty/{url}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetSpecialtyDescriptions_EndpointsReturnSuccessAndCorrectContentType()
        {
            //Arrange
            var entity = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();
            var Id = entity.SpecialtyId;

            // Act
            var response = await _client.GetAsync($"/api/Specialty/Descriptions/{Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetSpecialtyDescriptions_EndpointReturnNotFound()
        {
            //Arrange
            var Id = "abc";

            // Act            
            var response = await _client.GetAsync($"/api/Specialty/Descriptions/{Id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task AddSpecialtyAndInstitutionOfEducationToFavorite_EndpointsReturnOk()
        {
            //Arrange
            _specialtyInputAttribute.SetUserIdByGraduateUserIdForHttpContext();
            var favorite = _context.SpecialtyToInstitutionOfEducationToGraduates.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToInstitutionOfEducationToGraduates.Remove(favorite);
            _context.SaveChanges();

            var model = new SpecialtyAndInstitutionOfEducationPostApiModel
            {
                SpecialtyId = favorite.SpecialtyId,
                InstitutionOfEducationId = favorite.InstitutionOfEducationId
            };

            // Act            
            var response = await _client.PostAsync($"/api/Specialty/Favorites", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RemoveSpecialtyAndInstitutionOfEducationFromFavorite_EndpointReturnNoContent()
        {
            //Arrange
            var favorite = _context.SpecialtyToInstitutionOfEducationToGraduates.AsNoTracking().FirstOrDefault();
            var userId = _context.Graduates.AsNoTracking().Where(x => x.Id == favorite.GraduateId).FirstOrDefault().UserId;
            _specialtyInputAttribute.SetUserIdForHttpContext(userId);

            //Act
            var response = await _client.DeleteAsync(
                $"/api/Specialty/Favorites?specialtyId={favorite.SpecialtyId}&institutionOfEducationId={favorite.InstitutionOfEducationId}");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddSpecialtyFavorite_EndpointsReturnOk()
        {
            //Arrange
            _specialtyInputAttribute.SetUserIdByGraduateUserIdForHttpContext();
            var favorite = _context.SpecialtyToGraduates.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToGraduates.Remove(favorite);
            _context.SaveChanges();

            var model = favorite.SpecialtyId;

            // Act            
            var response = await _client.PostAsync($"/api/Specialty/Favorites/{favorite.SpecialtyId}", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RemoveSpecialtyFromFavorite_EndpointReturnNoContent()
        {
            //Arrange
            var favorite = _context.SpecialtyToGraduates.AsNoTracking().FirstOrDefault();
            var userId = _context.Graduates.AsNoTracking().Where(x => x.Id == favorite.GraduateId).FirstOrDefault().UserId;
            _specialtyInputAttribute.SetUserIdForHttpContext(userId);

            //Act
            var response = await _client.DeleteAsync(
                $"/api/Specialty/Favorites/{favorite.SpecialtyId}");

            //Assert
            response.EnsureSuccessStatusCode();
        }

    }
}
