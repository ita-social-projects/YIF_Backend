using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class AuthenticateResponseApiModelTests
    {
        [Theory]
        [InlineData("token", "refreshToken")]
        [InlineData("token")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_ShouldImplementParameters(string token, string refreshToken = null)
        {
            // Act
            var testObject = new AuthenticateResponseApiModel(token, refreshToken);
            //Assert
            Assert.Equal(token, testObject.Token);
            Assert.Equal(refreshToken, testObject.RefreshToken);
        }
    }
}
