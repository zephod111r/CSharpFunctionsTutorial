using FunctionTutorial.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionTutorial.Tests
{
    [TestClass]
    public class Function1Tests
    {
        [TestMethod]
        public void Run_ReturnsCorrectResponse()
        {
            // Arrange
            var logger = new LoggerFactory().CreateLogger<Function1>();
            var function = new Function1(logger);
            var request = new DefaultHttpContext().Request;

            // Act
            var result = function.Run(request) as ContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("text/html", result.ContentType);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsTrue(result.Content?.Contains("<html>"));
            Assert.IsTrue(result.Content?.Contains("<form action=\"Function2\" method=\"post\">"));
            Assert.IsTrue(result.Content?.Contains("<input class=\"inputbox\" type=\"text\" name=\"from\" placeholder=\"Your Name\">"));
            Assert.IsTrue(result.Content?.Contains("<input class=\"inputbox\" type=\"text\" name=\"message\" placeholder=\"Your message\">"));
            Assert.IsTrue(result.Content?.Contains("<button class=\"button\" type=\"submit\">Click me</button>"));
        }
    }
}
