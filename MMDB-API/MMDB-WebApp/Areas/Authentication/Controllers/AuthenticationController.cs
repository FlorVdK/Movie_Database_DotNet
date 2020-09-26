using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MMDB_WebApp.Areas.Authentication.Models;
using MMDB_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MMDB_WebApp.Helpers;
using Microsoft.Extensions.Localization;

namespace MMDB_WebApp.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStateHelper _stateHelper;
        private readonly IStringLocalizer<AuthenticationController> _localizer;

        public AuthenticationController(IHttpClientFactory httpClientFactory, IStateHelper stateHelper, IStringLocalizer<AuthenticationController> localizer)
        {
            _httpClientFactory = httpClientFactory;
            _stateHelper = stateHelper;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("/login", Name = "login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login", Name = "login")]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Login(UserLoginVM userLoginVM)
        {
            if (userLoginVM == null) { throw new ArgumentNullException(nameof(userLoginVM)); }

            if (ModelState.IsValid)
            {
                using var client = _httpClientFactory.CreateClient("MMDB_API");

                using var userContent = new StringContent(JsonSerializer.Serialize(userLoginVM), Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponseMessage = await client.PostAsync("users/login", userContent).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    UserVM userVM = await JsonSerializer.DeserializeAsync<UserVM>(responseStream);

                    try
                    {
                        _stateHelper.SetState(userVM, userLoginVM.RememberMe);
                    }
                    catch (Exception e)
                    {
                        _stateHelper.ClearState();
                        TempData["jwt"] = true;
                        TempData["jwtMessage"] = e.Message;
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    if (TempData.Keys.Contains("RedirectController"))
                    {
                        return RedirectToAction(TempData["RedirectActionMethod"] as string, TempData["RedirectController"] as string, new { area = "" });
                    }

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View(userLoginVM);
        }

        [HttpGet]
        [Route("/register", Name = "register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/register", Name = "register")]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Register(UserRegisterVM userRegisterVM)
        {
            if (userRegisterVM == null) { throw new ArgumentNullException(nameof(userRegisterVM)); }

            if (ModelState.IsValid)
            {
                using var client = _httpClientFactory.CreateClient("MMDB_API");

                using var userContent = new StringContent(JsonSerializer.Serialize(userRegisterVM), Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponseMessage = await client.PostAsync("users/register", userContent).ConfigureAwait(false);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    UserVM userVM = await JsonSerializer.DeserializeAsync<UserVM>(responseStream);
                    _stateHelper.SetState(userVM, true);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View(userRegisterVM);
        }

        [HttpGet]
        [Route("/logout", Name = "logout")]
        public IActionResult Logout()
        {
            _stateHelper.ClearState();
            return RedirectToAction("Index", "Home", new { area = ""});
        }
    }
}