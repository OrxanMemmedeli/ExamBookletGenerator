using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EBC.Core.Helpers.Authentication;

public static class CurrentUser
{
    private static IHttpContextAccessor _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static ClaimsPrincipal User => _httpContextAccessor?.HttpContext?.User;

    public static Guid UserId => Guid.Parse(User?.FindFirst(CustomClaimTypes.UserId)?.Value ?? Guid.Empty.ToString());
    public static bool IsAdmin => bool.Parse(User?.FindFirst(CustomClaimTypes.IsAdmin)?.Value ?? "false");
    public static bool IsManager => bool.Parse(User?.FindFirst(CustomClaimTypes.IsManager)?.Value ?? "false");
    public static string UserName => User?.FindFirst(CustomClaimTypes.UserName)?.Value ?? string.Empty;
    public static string FirstName => User?.FindFirst(CustomClaimTypes.FirstName)?.Value ?? string.Empty;
    public static string LastName => User?.FindFirst(CustomClaimTypes.LastName)?.Value ?? string.Empty;
    public static string FullName => User?.FindFirst(CustomClaimTypes.FullName)?.Value ?? string.Empty;
    public static string Roles => User?.FindFirst(CustomClaimTypes.Roles)?.Value ?? string.Empty;
    public static string OrganizationAddress => User?.FindFirst(CustomClaimTypes.OrganizationAddress)?.Value ?? string.Empty;
}