using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class DescriptionResponseApiModelTests
    {
        [Theory]
        [InlineData("message")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_ShouldImplementParameters(string message)
        {
            // Act
            var testObject = new DescriptionResponseApiModel(message);
            //Assert
            Assert.Equal(message, testObject.Message);
        }
    }
}
