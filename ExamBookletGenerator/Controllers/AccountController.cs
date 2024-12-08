using EBC.Core.Attributes.Authentication;
using EBC.Core.Helpers.Authentication;
using EBC.Core.Models;
using EBC.Core.Models.Dtos.Identities.User;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Data.Repositories.Abstract;
using ExamBookletGenerator.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace ExamBookletGenerator.Controllers;

[AllowAnonymous]
[AllowRoleFilter]
public class AccountController : Controller
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly GoogleReCaptureConfigModel _googleConfig;
    private readonly UserSessionManagerService _userSessionManagerService;

    public AccountController(
        IAppUserRepository appUserRepository,
        IOptions<GoogleReCaptureConfigModel> googleConfig,
        UserSessionManagerService userSessionManagerService)
    {
        _appUserRepository = appUserRepository;
        _googleConfig = googleConfig.Value;
        _userSessionManagerService = userSessionManagerService;
    }

    public IActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel login, string returnUrl)
    {

        bool isCaptchaValid = await IsReCaptchValidV3Async(login.captcha);
        if (ModelState.IsValid && isCaptchaValid)
        {
            (Result<UserLoginResponseDTO> result, List<Claim> claims) = await _appUserRepository.GetLoginInfo(login.UserName, login.Password);

            if (result.IsSuccess && claims.Any())
            {
                var useridentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(useridentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                return !string.IsNullOrEmpty(returnUrl)
                    ? Redirect(returnUrl)
                    : Redirect("/Admin");
            }
        }
        TempData["LoginMessage"] = "Uğursuz əməliyat";
        return View();
    }




    public IActionResult Denied()
    {
        return View();
    }

    public IActionResult LogOut()
    {
        _userSessionManagerService.RemoveUser(CurrentUser.UserId);
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task<bool> IsReCaptchValidV3Async(string captchaResponse)
    {
        if (string.IsNullOrWhiteSpace(captchaResponse))
            throw new ArgumentException("Captcha response cannot be null or empty", nameof(captchaResponse));

        var secretKey = _googleConfig.Secret;
        var apiUrl = "https://www.google.com/recaptcha/api/siteverify";

        using var httpClient = new HttpClient();
        var values = new Dictionary<string, string>
        {
            { "secret", secretKey },
            { "response", captchaResponse }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await httpClient.PostAsync(apiUrl, content);
        if (!response.IsSuccessStatusCode)
            return false;

        var responseString = await response.Content.ReadAsStringAsync();
        var jResponse = JObject.Parse(responseString);
        return jResponse.Value<bool>("success");
    }
}

