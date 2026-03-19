using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Shared;

namespace Api.Functions;

public class MessageFunction
{
    [Function("Message")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "message")] HttpRequest req)
    {
        string? user = null;

        // SWA injects x-ms-client-principal header with base64-encoded user info
        if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
        {
            var decoded = Convert.FromBase64String(header!);
            var principal = JsonSerializer.Deserialize<ClientPrincipal>(decoded, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            user = principal?.UserDetails;
        }

        var message = new ApiMessage
        {
            Text = user is not null
                ? $"Hello, {user}! Welcome back."
                : "Hello! Sign in to get a personalized greeting.",
            Timestamp = DateTime.UtcNow,
            User = user
        };

        return new OkObjectResult(message);
    }
}
