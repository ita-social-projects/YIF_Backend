using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SchoolAdminApiModelTests
    {
        [Theory]
        [InlineData("SchoolName", "Email", "Password")]
        [InlineData(null, "Null", "0")]
        [InlineData("", "", "")]
        public void Ctor_ShouldImplementParameters(string SchoolName, string Email, string Password)
        {
            //Act
            var actualApiModel = new SchoolAdminApiModel
            {
                SchoolName = SchoolName,
                Email = Email,
                Password = Password
            };

            //Assert
            Assert.Equal(SchoolName, actualApiModel.SchoolName);
            Assert.Equal(Email, actualApiModel.Email);
            Assert.Equal(Password, actualApiModel.Password);
        }
    }
}
