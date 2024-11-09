using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace EBC.Core.Helpers.Authentication;

public static class ClaimsHelper
{
    /// <summary>
    /// İstifadəçi məlumatlarını alaraq Claims listinə çevirir.
    /// </summary>
    /// <param name="user">Məlumatları `Claim` kimi saxlanılacaq istifadəçi obyekti.</param>
    /// <returns>Claims listi.</returns>
    public static List<Claim> GetUserClaims(CustomUser user)
    {
        return new List<Claim>
        {
            new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
            new Claim(CustomClaimTypes.IsAdmin, user.IsAdmin.ToString()),
            new Claim(CustomClaimTypes.IsManager, user.IsManager.ToString()),
            new Claim(CustomClaimTypes.UserName, user.UserName),
            new Claim(CustomClaimTypes.FirstName, user.FirstName),
            new Claim(CustomClaimTypes.LastName, user.LastName),
            new Claim(CustomClaimTypes.FullName, user.FullName),
            new Claim(CustomClaimTypes.Roles, user.Roles),
            new Claim(CustomClaimTypes.OrganizationAddress, user.OrganizationAddress)
        };
    }

    /// <summary>
    /// ClaimsPrincipal yaradır, istifadəçinin məlumatlarını Cookie-based autentifikasiyada saxlamaq üçün istifadə olunur.
    /// </summary>
    /// <param name="user">Claim-lərə əlavə ediləcək CustomUser obyekti.</param>
    /// <returns>ClaimsPrincipal obyekti.</returns>
    public static ClaimsPrincipal CreatePrincipal(CustomUser user)
    {
        var identity = new ClaimsIdentity(GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}


/*
 Numune 

    var claimsPrincipal = ClaimsHelper.CreatePrincipal(user);

    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

 */