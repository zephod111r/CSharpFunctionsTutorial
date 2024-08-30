using FunctionTutorial.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Text;

namespace FunctionTutorial.Tests
{
    [TestClass]
    public class Function2Tests : TestSetup
    {
        [TestMethod]
        public async Task Run_ReturnsOkResult()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var logger = new Logger<Function2>(new LoggerFactory());

                var mockRequest = new DefaultHttpContext().Request;
                var formFields = new Dictionary<string, StringValues>
                {
                    { "from", "John Doe" },
                    { "message", "Hello, World!" }
                };
                var form = new FormCollection(formFields);

                mockRequest.Form = form;
                mockRequest.Body = new MemoryStream();
                mockRequest.Body.Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { from = "John Doe", message = "Hello, World!" })));
                mockRequest.Body.Flush();
                mockRequest.Body.Seek(0, SeekOrigin.Begin);

                var function = new Function2(logger);

                // Act
                IActionResult result = await function.Run(mockRequest);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkResult));
            }
        }
    }
}
