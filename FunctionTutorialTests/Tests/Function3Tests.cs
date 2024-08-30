using FunctionTutorial.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionTutorial.Tests
{
    [TestClass]
    public class Function3Tests
    {
        private readonly ILogger<Function3> _logger;
        private readonly Function3 _function;

        public Function3Tests()
        {
            //_logger = new StubILogger<Function3>();
            //_function = new Function3(_logger);
        }

        [TestMethod]
        public void Run_ReturnsExpectedContentResult()
        {
            /*
            // Arrange
            using (ShimsContext.Create())
            {
                var request = new ShimHttpRequest();
                ShimStatusCodes.Status200OKGet = () => 200;

                // Act
                var result = _function.Run(request.Instance);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ContentResult));

                var contentResult = (ContentResult)result;
                Assert.AreEqual("text/css", contentResult.ContentType);
                Assert.AreEqual(200, contentResult.StatusCode);
                Assert.AreEqual(@"
                        body {
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                            color: #333333;
                        }
                        .inputbox { 
                            width: 200px;
                            height: 30px;
                            border: 1px solid #cccccc;
                            border-radius: 4px;
                            padding: 5px;
                        }
                        .button {
                            background-color: #007bff;
                            color: #ffffff;
                            border: none;
                            border-radius: 4px;
                            padding: 10px 20px;
                            cursor: pointer;
                        }", contentResult.Content);
            }
            */
        }
    }
}
