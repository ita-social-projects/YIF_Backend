using Xunit;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class ResponseApiModelTests
    {
        [Theory]
        [InlineData(true, "message")]
        [InlineData(true)]
        [InlineData(false, "message")]
        [InlineData(false)]
        public void Ctor_ShouldImplementParameters(bool success, string message = null)
        {
            // Act
            var testObject = new ResponseApiModel<double>(success, message);
            //Assert
            Assert.Equal(success, testObject.Success);
            Assert.Equal(message, testObject.Message);
            Assert.Equal(typeof(double), testObject.Object.GetType());
        }

        [Theory]
        [InlineData(true, "message")]
        [InlineData(true)]
        [InlineData(false, "message")]
        [InlineData(false)]
        public void Set_ShouldSetStatusAndMessage_ReturnResponseApiModel(bool success, string message = null)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Set(success, message);
            //Assert
            Assert.Equal(success, result.Success);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("Word", true, "message")]
        [InlineData(7, true)]
        [InlineData(2.2, false)]
        [InlineData(new int[] { 2, 3 }, false)]
        [InlineData(false, true)]
        [InlineData(new char[] { 'a', 'b' }, true)]
        public void Set_ShouldSetStatusAndObjectAndMessage_ReturnResponseApiModel(object obj, bool success, string message = null)
        {
            // Arrange
            var testObject = new ResponseApiModel<object>();
            // Act
            var result = testObject.Set(obj, success, message);
            //Assert
            Assert.Equal(obj.GetType(), result.Object.GetType());
            Assert.Equal(success, result.Success);
            Assert.Equal(obj, result.Object);
            Assert.Equal(message, result.Message);
        }
    }
}
