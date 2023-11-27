using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Middleware.Requirements;

namespace Portfolio.Backend;

public class JwtCookieAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new BaseControllerResponse
            {
                message = "Unauthorized Access",
                succeed = false,
                reason = "Invalid Token"
            };
            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
            return;

        }

        if (authorizeResult.Forbidden)
        {

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = new BaseControllerResponse
            {
                message = "Forbidden",
                succeed = false,
                reason = "Resource not available to this user"
            };
            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
            return;
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
