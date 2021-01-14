using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversityFilterResponseApiModelTests
    {
        [Theory]
        [InlineData("Direction", "Name", "Speciality","Image")]
        [InlineData(null, "Null", "Sho","sls")]
        [InlineData("", "", "", "")]
        public void Ctor_ShouldImplementParameters(string Id, string Name, string Description, string ImagePath)
        {
            var apiModel = new UniversityFilterResponseApiModel(Id, Name, Description, ImagePath);

            Assert.Equal(Id, apiModel.Id);
            Assert.Equal(Name, apiModel.Name);
            Assert.Equal(Description, apiModel.Description);
            Assert.Equal(ImagePath, apiModel.ImagePath);
        }

    }
}
