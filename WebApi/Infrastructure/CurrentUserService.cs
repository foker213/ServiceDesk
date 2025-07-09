using Microsoft.AspNetCore.Http;
using ServiceDesk.Application.IServices;

namespace ServiceDesk.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => Convert.ToInt32(GetClaimValue("ID"));

    private string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?
            .User?
            .Claims?
            .FirstOrDefault(c => c.Type == claimType)?
            .Value;
    }
}

