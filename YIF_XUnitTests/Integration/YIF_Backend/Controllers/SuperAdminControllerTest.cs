using System.Threading.Tasks;
using Xunit;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SuperAdminControllerTest : IClassFixture<BaseTestServerFixture>
    {
        private readonly BaseTestServerFixture _fixture;

        public SuperAdminControllerTest(BaseTestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("", "", "", "", "", "", "", 0, 0)]
        public async Task AddUniversity_EndpointsReturnSuccess(
            string Name,
            string Abbreviation,
            string Site,
            string Address,
            string Phone,
            string Email,
            string Description,
            float Lat,
            float Lon)
        {
            var postRequest = new
            {
                Url = "api/SuperAdmin/AddUniversity?",
                Body = new
                {
                    Name = "Product",
                    Abbreviation = "Product",
                    Site = "Product",
                    Address = "Product",
                    Phone = "Product",
                    Email = "Product",
                    Description = "Product",
                    Lat = 0.0,
                    Lon = 0.0,
                }
            };
            // Act            
            var response = await _fixture.Client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //var response = await _fixture.Client.GetAsync($"api/SuperAdmin/AddUniversity?" +
            //    $"Name={Name}&Abbreviation={Abbreviation}&Site={Site}&Address={Address}&Phone={Phone}&Email={Email}" +
            //    $"&Description={Description}&Description={Description}&Description={Description}&Lat={Lat}&Lon={Lon}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
