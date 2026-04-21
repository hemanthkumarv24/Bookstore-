using Bookstore_.Data;

namespace Bookstore_.Middleware;

public class TokenQueueValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Validate only authenticated requests by checking token presence in private queue.
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var authHeader = context.Request.Headers.Authorization.ToString();
            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader["Bearer ".Length..].Trim()
                : string.Empty;

            if (string.IsNullOrWhiteSpace(token) || !InMemoryStore.ActiveTokens.Contains(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Token is not in active queue." });
                return;
            }
        }

        await next(context);
    }
}
