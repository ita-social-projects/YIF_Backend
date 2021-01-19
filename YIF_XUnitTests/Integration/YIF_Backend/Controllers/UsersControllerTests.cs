using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_Backend;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class UsersControllerTests 
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public UsersControllerTests()
        {
            var clientOptions = new WebApplicationFactoryClientOptions();
            clientOptions.BaseAddress = new Uri("https://localhost:44324/api/Users/");

            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(clientOptions);
        }

        [Theory]
        [InlineData("")]
        [InlineData("6f08c1b6-468d-4c44-afeb-488ded1ccb98")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("00f5e3b3-fa90-4e74-856f-cbd41f178520")]
        public async Task Get_EndpointReturnNotFound(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("d")]
        public async Task Get_EndpointReturnBadRequest(string url)
        {
            // Act            
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("intelIdea@gmail.com")]
        public async Task Send_ResetPassword_IfEmail_Correct(string email)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ResetPasswordByEmailApiModel
            {
                UserEmail = email
            }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("ResetPassword", content);

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
            var content = new StringContent(JsonConvert.SerializeObject(new ResetPasswordByEmailApiModel
            {
                UserEmail = email
            }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("ResetPassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3","QWerty-1","QWerty-12","QWerty-12",
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-12", "QWerty-1", "QWerty-1",
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async Task ChangePassword_With_CurrentParameters(string id,
            string oldPassword,
            string newPassword,
            string confirmPassword,
            string recaptcha)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ChangePasswordApiModel
            {
                UserId = id,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword,
                RecaptchaToken = recaptcha
            }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("ChangePassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
              response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a5", "QWerty-1", "QWerty-12", "QWerty-12", // ID incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-1222", "QWerty-1", "QWerty-1", // OldPassword incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-1", "Q", "QWerty-1", // NewPassword incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-1", "QWerty-12", "QWerty-122", // Confirm password incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        [InlineData("0a8404ae-53ff-4ed4-bf76-994d915123a3", "QWerty-1", "QWerty-12", "QWerty-12", // Recaptcha incorrect
            "03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRowkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs")]
        public async Task ChangePassword_With_InCurrentParameters(string id,
            string oldPassword,
            string newPassword,
            string confirmPassword,
            string recaptcha)
        {
            // Act
            var content = new StringContent(JsonConvert.SerializeObject(new ChangePasswordApiModel
            {
                UserId = id,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmPassword,
                RecaptchaToken = recaptcha
            }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("ChangePassword", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
              response.Content.Headers.ContentType.ToString());
        }
    }
}
