using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FunctionTutorial.Functions
{
    public class Login
    {
        private readonly ILogger<Login> _logger;

        public Login(ILogger<Login> logger)
        {
            _logger = logger;
        }

        [Function("login")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Read the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Deserialize the request body to a dynamic object
            dynamic data = JsonSerializer.Deserialize<dynamic>(requestBody);

            // Get the username and password from the request body
            string username = data["username"];
            string password = data["password"];

            // Validate the user
            bool isValidUser = ValidateUser(username, password);

            if (isValidUser)
            {
                return new OkObjectResult("Welcome to Azure Functions!");
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        private bool ValidateUser(string username, string password)
        {
            // Add your user validation logic here
            // For example, check if the username and password match a user in the database
            // Return true if the user is valid, false otherwise

            // TODO: Add proper validation
            return (username == "admin" && password == "password");
        }
    }
}
