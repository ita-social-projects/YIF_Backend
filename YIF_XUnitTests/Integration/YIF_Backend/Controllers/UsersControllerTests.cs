using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class UsersControllerTests : TestServerFixture
    {
        private static string _correctUserId;
        private readonly UserInputAttribute _userInputAttribute;
        public UsersControllerTests(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _correctUserId = _context.Users.First().Id;
            _userInputAttribute = new UserInputAttribute(_context);
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var id = _correctUserId;

            // Act            
            var response = await _client.GetAsync($"api/Users/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("00f5e3b3-fa90-4e74-856f-cbd41f178520")]
        public async Task Get_EndpointReturnNotFound(string id)
        {
            // Act            
            var response = await _client.GetAsync($"api/Users/{id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Send_ResetPassword_IfEmail_Correct()
        {
            // Arrange
            var email = _context.Users.First().Email;

            // Act
            var response = await _client.PostAsync($"api/Users/Reset?userEmail={email}", null);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("notExist@gmail.com")]
        public async Task Send_ResetPassword_IfEmail_InCorrect(string email)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new EmailApiModel
            {
                UserEmail = email
            }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Users/Reset", content);


            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("QWerty-1","QWerty-12","QWerty-12",
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("QWerty-12", "QWerty-1", "QWerty-1",
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async Task ChangePassword_With_CurrentParameters(
            string oldPassword,
            string newPassword,
            string confirmPassword,
            string recaptcha)
        {
            //Arrange
            _userInputAttribute.SetUserIdForHttpContext();
            var content = new StringContent(JsonConvert.SerializeObject(new ChangePasswordApiModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword,
                RecaptchaToken = recaptcha
            }), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("api/Users/ChangePassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
              response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("QWerty-1222", "QWerty-1", "QWerty-1", // OldPassword incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("QWerty-1", "Q", "QWerty-1", // NewPassword incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("QWerty-1", "QWerty-12", "QWerty-122", // Confirm password incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("QWerty-1", "QWerty-12", "QWerty-12", // Recaptcha incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRowkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async Task ChangePassword_With_IncorrentParameters(
            string oldPassword,
            string newPassword,
            string confirmPassword,
            string recaptcha)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ChangePasswordApiModel
            {
                UserId = _correctUserId,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword,
                RecaptchaToken = recaptcha
            }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("api/Users/ChangePassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
              response.Content.Headers.ContentType.ToString());
        }
        
    }
}
