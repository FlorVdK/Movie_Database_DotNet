using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMDB_WebApp.Models;

namespace MMDB_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MovieVM> movies;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("movies/recent").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                movies = await JsonSerializer.DeserializeAsync<IEnumerable<MovieVM>>(responseStream);
                movies.OrderByDescending(c => c.ReleaseDate).Take(5);
            }
            else
            {
                movies = Array.Empty<MovieVM>();
            }

            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCulture(string culture)
        {
            string controller = null;
            string action = null;
            string area = null;
            int? id = null;

            if (TempData["Controller"] != null)
            {
                controller = TempData["Controller"].ToString();

                if (TempData["Controller"].ToString() != "Authentication")
                {
                    TempData["Area"] = "";
                }
            }
            else
            {
                controller = "Home";
            }

            if (TempData["Action"] != null)
            {
                action = TempData["Action"].ToString();
            }
            else
            {
                action = "Index";
            }

            if (TempData["Area"] != null)
            {
                area = TempData["Area"].ToString();
            }
            else
            {
                area = "";
            }

            if (TempData["Id"] != null)
            {
                id = int.Parse(TempData["Id"].ToString());
            }
            else
            {
                id = null;
            }

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(5)
            };
            Request.HttpContext.Response.Cookies.Append("culture", culture, cookieOptions);
            Request.HttpContext.Session.SetString("culture", culture);
            return RedirectToAction(action, controller, new { area, id });
        }
    }
}
