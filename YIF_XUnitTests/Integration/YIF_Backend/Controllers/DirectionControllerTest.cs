using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF_Backend;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class DirectionControllerTest : IClassFixture<BaseTestServerFixture>
    {
        private readonly BaseTestServerFixture _fixture;

        public DirectionControllerTest(BaseTestServerFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async Task GetAll_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act            
            var response = await _fixture.Client.GetAsync("api/Specialty/All");
            response.EnsureSuccessStatusCode();
            var models = JsonConvert.DeserializeObject<IEnumerable<SpecialtyResponseApiModel>>(await response.Content.ReadAsStringAsync());
            // Assert
            Assert.NotEmpty(models);
        }
    }
}
