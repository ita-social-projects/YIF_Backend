using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class InstitutionOfEducationFilterResponseApiModelTests
    {
        [Theory]
        [InlineData("Direction", "Name", "Specialty","Image")]
        [InlineData(null, "Null", "Sho","sls")]
        [InlineData("", "", "", "")]
        public void Ctor_ShouldImplementParameters(string Id, string Name, string Description, string ImagePath)
        {
            var apiModel = new InstitutionOfEducationResponseApiModel
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ImagePath = ImagePath
            };

            Assert.Equal(Id, apiModel.Id);
            Assert.Equal(Name, apiModel.Name);
            Assert.Equal(Description, apiModel.Description);
            Assert.Equal(ImagePath, apiModel.ImagePath);
        }

    }
}
