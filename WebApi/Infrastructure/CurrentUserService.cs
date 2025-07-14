using Microsoft.AspNetCore.Http;
using ServiceDesk.Application.IServices;
using System.Security.Claims;

namespace ServiceDesk.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => Convert.ToInt32(GetClaimValue(ClaimTypes.NameIdentifier));

    private string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?
            .User?
            .Claims?
            .FirstOrDefault(c => c.Type == claimType)?
            .Value;
    }
}

