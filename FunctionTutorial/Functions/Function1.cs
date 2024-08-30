using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionTutorial.Functions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            string body = @"<html>
                                <head>
                                    <link rel=""stylesheet"" href=""Function3"" />
                                    <style>
                                        .message-list {
                                            list-style-type: none;
                                            padding: 0;
                                        }
                                        .message-item {
                                            border: 1px solid #ccc;
                                            margin: 10px 0;
                                            padding: 10px;
                                            border-radius: 5px;
                                        }
                                        .message-item h3 {
                                            margin: 0 0 10px 0;
                                        }
                                    </style>
                                </head>
                                <body>
                                    
                                    <div id=""history"" class=""message-list""></div>
                                    <form id=""postMessage"" action=""Function2"" method=""post"">
                                        <input class=""inputbox"" type=""text"" name=""from"" placeholder=""Your Name"">
                                        <input class=""inputbox"" type=""text"" name=""message"" placeholder=""Your message"">
                                        <button class=""button"" type=""submit"">Click me</button>
                                    </form>
                                    
                                    <script>
                                        document.getElementById(""postMessage"").addEventListener(""submit"", function(event) {
                                            event.preventDefault(); // Prevent the default form submission behavior

                                            // Perform an AJAX request to submit the form data
                                            var xhr = new XMLHttpRequest();
                                            xhr.open(""POST"", ""Function2"", true);
                                            xhr.setRequestHeader(""Content-Type"", ""application/json"");
                                            xhr.onreadystatechange = function() {
                                                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                                                    // Handle the response from the server
                                                    console.log(xhr.responseText);
                                                }
                                            };
                                            var data = {
                                                from: event.target.from.value,
                                                message: event.target.message.value
                                            };
                                            xhr.send(JSON.stringify(data));

                                            setTimeout(listenForMessages, 5000);
                                        });

                                        const createMessageHtml = (message) => {
                                            return `
                                                <div class=""message-item"" id=""${message.messageId}"">
                                                    <h3>From: ${message.from}</h3>
                                                    <p><strong>Message:</strong> ${message.message}</p>
                                                    <p><strong>Timestamp:</strong> ${new Date(message.timestamp).toLocaleString()}</p>
                                                </div>
                                            `;
                                        }

                                        // Calculate the timestamp for the last 180 days
                                        let currentDate = new Date();

                                        // Subtract 180 days in milliseconds (180 days * 24 hours * 60 minutes * 60 seconds * 1000 milliseconds)
                                        let pastDate = new Date(currentDate.getTime() - (180 * 24 * 60 * 60 * 1000));

                                        // Convert the past date to UTC
                                        let pastDateUTC = new Date(Date.UTC(pastDate.getUTCFullYear(), pastDate.getUTCMonth(), pastDate.getUTCDate(), pastDate.getUTCHours(), pastDate.getUTCMinutes(), pastDate.getUTCSeconds()));

                                        const listenForMessages = () => {

                                            var historyBox = document.getElementById(""history"");
                                            // Perform an AJAX request to get the history data
                                            var xhr = new XMLHttpRequest();
                                            xhr.open(""GET"", ""Function4"", true);
                                            
                                            xhr.setRequestHeader(""Last-Request-Timestamp"", pastDateUTC.toUTCString());
                                            xhr.onreadystatechange = function() {
                                                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                                                    // Handle the response from the server
                                                    var historyData = JSON.parse(xhr.responseText);

                                                    // Populate the history box with the data
                                                    for (var i = 0; i < historyData.length; i++) {
                                                        var historyItem = document.getElementById(historyData.messageId);
                                                        if(!historyItem) {
                                                            historyItem = document.createElement('div');
                                                            historyBox.appendChild(historyItem);
                                                        }
                                                        // Convert the timestamp string to a Date object
                                                        var messageTimestamp = Date.parse(historyData.timestamp);

                                                        // Check if the message timestamp is later than pastDateUTC
                                                        pastDateUTC = messageTimestamp;
                                                        historyItem.outerHTML = createMessageHtml(historyData[i]);
                                                    }
                                                }
                                            };
                                            xhr.send();
                                        }

                                        // Call the populateHistory function initially
                                        listenForMessages();
                                    </script>
                                </body>
                                </html>";

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new ContentResult
            {
                Content = body,
                ContentType = "text/html",
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
