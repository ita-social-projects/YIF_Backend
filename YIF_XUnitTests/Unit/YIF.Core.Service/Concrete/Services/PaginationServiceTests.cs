using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class PaginationServiceTests
    {
        private static readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();

        private static readonly IPaginationService _paginationService = new PaginationService(_resourceManager.Object);

        [Fact]
        public void GetPageFromCollection_ShouldReturnPage_IfEverythingOk()
        {
            // Arrange  
            var collection = GetCollection(10);
            var pageModel = new PageApiModel
            {
                Page = 1,
                PageSize = 5,
                Url = "testhost"
            };

            // Act
            var result = _paginationService.GetPageFromCollection(collection, pageModel);

            // Assert
            Assert.IsType<PageResponseApiModel<int>>(result);
            Assert.True(result.ResponseList is IEnumerable<int>);
            Assert.Equal(result.ResponseList.Count(), pageModel.PageSize);
            Assert.Equal(result.CurrentPage, pageModel.Page);
            Assert.Null(result.PrevPage);
            Assert.NotEmpty(result.NextPage);
        }

        [Theory]
        // If the page size is less than or equal to 0
        [InlineData(10, 0)]
        // If the page is less than or equal to 0
        [InlineData(0, 10)]        
        // If the page is larger than the (collection size / page size)
        [InlineData(int.MaxValue, int.MaxValue)]
        public void GetPageFromCollection_ShouldThrowBadRequestException_IfIncorrectPageSettings(int page, int pageSize)
        {
            // Arrange  
            var collection = GetCollection(10);
            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = "testhost"
            };

            // Act
            Func<PageResponseApiModel<int>> act = () => _paginationService.GetPageFromCollection(collection, pageModel);

            // Assert
            Assert.Throws<BadRequestException>(act);
        }

        /// <summary>
        /// Generate list of numbers from 1 to size
        /// </summary>
        private IEnumerable<int> GetCollection(int size)
        {
            var list = new int[size];
            for (int i = 0; i < size; i++)
            {
                list[i] = i + 1;
            }
            return list;
        }
    }
}
