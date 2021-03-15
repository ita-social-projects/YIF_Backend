using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;
using YIF.Core.Service.Concrete;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete
{
    public class ConvertImageApiModelToPathTest
    {
        [Theory]
        [InlineData("","")]
        [InlineData("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAYAAACp8Z5+AAAATUlEQVQYV2NsCJsfwMHDPpOBgYHhx5ef6YyVoVNfGllri7GwMjOcOnDpFWNjxIKX6iZyYv///We4de7xK0aQFmYWppm///xlYGJgTAcAcGcbPQnK4IEAAAAASUVORK5CYII=", "")]
        [InlineData("", "C:\\")]
        public void FromBase64ToImageFilePath_WrongInput(string base64, string path)
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<ArgumentNullException>(() => ConvertImageApiModelToPath.FromBase64ToImageFilePath(base64, path));
        }
    }
}
