using FunctionTutorial.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.QualityTools.Testing.Fakes;
using FunctionTutorial.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace FunctionTutorial.Tests
{
    [TestClass]
    public class Function2Tests
    {
        [TestMethod]
        public async Task Run_ReturnsOkObjectResult()
        {
            
            // Arrange
            using (ShimsContext.Create())
            {
                var logger = new Microsoft.VisualStudio.TestTools.UnitTesting.Logging.Logger<Function2>();
                var request = new DefaultHttpContext(); var formFields = new Dictionary<string, StringValues>
                {
                    { "from", "John Doe" },
                    { "message", "Hello, World!" }
                };
                var form = new FormCollection(formFields);


                request.FormOptions = () => form;

                var function = new Function2(logger);

                // Act
                var result = await function.Run(request);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            }
            
        }
    }
}
