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

        var isValid = IsReCaptchValidV3(login.captcha);

        if (ModelState.IsValid && isValid)
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

    private bool IsReCaptchValidV3(string captchaResponse)
    {
        var result = false;
        var secretKey = _googleConfig.Secret;
        var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
        var request = (HttpWebRequest)WebRequest.Create(requestUri);

        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                JObject jResponse = JObject.Parse(stream.ReadToEnd());
                var isSuccess = jResponse.Value<bool>("success");
                result = (isSuccess) ? true : false;
            }
        }
        return result;
    }
}

