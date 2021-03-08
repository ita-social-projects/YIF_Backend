using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Integration.Fixture
{
    public class SuperAdminControllerTest : TestServerFixture
    {
        public SuperAdminControllerTest(ApiWebApplicationFactory fixture)
          : base(fixture) { }

        //private SuperAdminController _superAdminController;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        // new Claim(ClaimTypes.Name, "UserName"),
        // new Claim(ClaimTypes.Role, "SuperAdmin")
        //    }));

        //    _superAdminController = new SuperAdminController();
        //    _superAdminController.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = user }
        //    };
        //}


        //[Theory]
        //[InlineData("api/Direction/All")]
        //[InlineData("api/Direction/All?page=1")]
        //[InlineData("api/Direction/All?page=1&pageSize=10")]
        //[InlineData("api/Direction/All?DirectionName=Інформаційні технології")]
        //[InlineData("api/Direction/All?DirectionName=Інформаційні технології&SpecialtyName=Кібербезпека&UniversityName=Київський політехнічний інститут імені Ігоря Сікорського&UniversityAbbreviation=КПІ")]
        //[InlineData("api/Direction/All?DirectionName=Інформаційні технології&SpecialtyName=Кібербезпека&UniversityName=Київський політехнічний інститут імені Ігоря Сікорського&UniversityAbbreviation=КПІ&page=1&pageSize=10")]
        //public async Task GetAll_EndpointsReturnSuccessAndCorrectContentObject(string endpoint)
        //{
        //    // Act            
        //    var response = await _client.GetAsync(endpoint).;

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    var stringResponse = await response.Content.ReadAsStringAsync();
        //    var models = JsonConvert.DeserializeObject<IEnumerable<DirectionResponseApiModel>>(stringResponse);
        //    Assert.NotEmpty(models);
        //}
    }
}