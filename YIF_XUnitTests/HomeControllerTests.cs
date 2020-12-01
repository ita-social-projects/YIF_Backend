using System;
using Xunit;
using YIF.Core.Data.Entities;
using YIF_Backend.Controllers;

namespace YIF_XUnitTests
{
    public class HomeControllerTests
    {
        public readonly EFDbContext context;
        [Fact]
        public void GetUsersReturnsNotNull()
        {
            // Arrange
            HomeController controller = new HomeController(context);

            // Act
            var result = controller.GetUsers();

            // Assert
            Assert.NotNull(result);
        }
    }
}
