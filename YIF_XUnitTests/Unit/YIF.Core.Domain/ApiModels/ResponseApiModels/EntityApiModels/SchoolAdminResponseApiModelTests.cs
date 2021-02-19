using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels.EntityApiModels
{
    public class SchoolAdminResponseApiModelTests
    {
        [Theory]
        [InlineData("Id", "SchoolId", "SchoolName")]
        [InlineData(null, "Null", "0")]
        [InlineData("", "", "")]
        public void Ctor_ShouldImplementParameters(string Id, string SchoolId, string SchoolName)
        {
            //Act
            var actualApiModel = new SchoolAdminResponseApiModel
            {
                Id = Id,
                SchoolId = SchoolId,
                SchoolName = SchoolName
            };

            //Assert
            Assert.Equal(Id, actualApiModel.Id);
            Assert.Equal(SchoolId, actualApiModel.SchoolId);
            Assert.Equal(SchoolName, actualApiModel.SchoolName);
        }
    }
}
