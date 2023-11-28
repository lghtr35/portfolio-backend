namespace Portfolio.Backend.Middleware.Handlers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Backend.Middleware.Requirements;

public class JwtCookieRequirementHandler : IAuthorizationHandler
{
    private readonly Services.Interfaces.IAuthenticationService _authenticationService;

    public JwtCookieRequirementHandler(Services.Interfaces.IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pending = context.PendingRequirements.ToList();
        foreach (var requirement in pending)
        {
            if (requirement is JwtCookieRequirement)
            {
                bool isValid = false;
                try
                {
                    var token = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Not Found";
                    isValid = _authenticationService.ValidateToken(token);
                }
                catch (Exception err)
                {
                    isValid = false;
                }
                if (isValid)
                {
                    context.Succeed(requirement);
                }
            }
        }
        return Task.CompletedTask;
    }

}
