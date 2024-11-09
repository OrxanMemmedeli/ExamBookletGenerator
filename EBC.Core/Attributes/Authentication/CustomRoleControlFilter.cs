using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EBC.Core.Helpers.Authentication;

namespace EBC.Core.Attributes.Authentication;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class CustomRoleControlFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // `AllowRoleFilterAttribute` varsa, rol yoxlaması olmadan davam edir.
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowRoleFilterAttribute>().Any())
        {
            base.OnActionExecuting(context);
            return;
        }

        // İstifadəçi üçün URL quruluşu yaradılır.
        string url = BuildUrl(context);

        // `Claim` yoxlaması ilə istifadəçinin icazəsi olub-olmadığını yoxlayırıq.
        if (!UserHasAccess(context, url))
            context.Result = new RedirectToActionResult("Denied", "Account", new { area = "" });
        else
            base.OnActionExecuting(context);
    }

    /// <summary>
    /// İstifadəçinin müəyyən bir URL-ə giriş icazəsi olub-olmadığını yoxlayır.
    /// </summary>
    /// <param name="context">İstifadəçi konteksti.</param>
    /// <param name="url">İstifadəçinin giriş etməyə çalışdığı URL.</param>
    /// <returns>İcazə varsa true, yoxdursa false qaytarır.</returns>
    private bool UserHasAccess(ActionExecutingContext context, string url)
    {
        var claim = context.HttpContext.User.FindFirst(CustomClaimTypes.OrganizationAddress);
        if (claim == null)
        {
            return false;
        }

        var allowedUrls = claim.Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return allowedUrls.Contains(url, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kontekstdən istifadə edərək URL strukturunu yaradır.
    /// </summary>
    /// <param name="context">İstifadəçi konteksti.</param>
    /// <returns>Tam URL strukturunu qaytarır.</returns>
    private string BuildUrl(ActionExecutingContext context)
    {
        var areaName = context.RouteData.Values["area"]?.ToString();
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();

        return areaName != null
            ? $"/{areaName}/{controllerName}/{actionName}"
            : $"/{controllerName}/{actionName}";
    }
}