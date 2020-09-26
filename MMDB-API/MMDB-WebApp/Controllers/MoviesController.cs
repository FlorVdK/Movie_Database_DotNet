using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MMDB_WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using PagedList;

namespace MMDB_WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MoviesController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string SearchString)
        {
            IEnumerable<MovieVM> movies;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("movies").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                movies = await JsonSerializer.DeserializeAsync<IEnumerable<MovieVM>>(responseStream);
            }
            else
            {
                movies = Array.Empty<MovieVM>();
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            MoviePutVM movie;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("movies/" + id).ConfigureAwait(false);
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                movie = await JsonSerializer.DeserializeAsync<MoviePutVM>(responseStream);

            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM movie)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MMDB_API");

                string uniqueFileName = UploadedFile(movie);

                MoviePutVM moviePost = new MoviePutVM
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    DirectorId = movie.DirectorId,
                    Poster = uniqueFileName
                };

                var movieContent = new StringContent(JsonSerializer.Serialize(moviePost), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("movies", movieContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectOnError(response, "Create", "create an movie", "creating an movie");
            }

            return View(movie);
        }

        private ActionResult RedirectOnError(HttpResponseMessage response, string actionMethod, string forbiddenMessage, string errorMessage)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["RedirectController"] = "Movie";
                TempData["RedirectActionMethod"] = actionMethod;
                return RedirectToRoute("login");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                TempData["forbidden"] = true;
                TempData["forbiddenMessage"] = forbiddenMessage;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = true;
                TempData["errorMessage"] = errorMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        private string UploadedFile(MovieVM movie)
        {
            string uniqueFileName = null;

            if (movie.PosterFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Posters");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + movie.PosterFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    movie.PosterFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("movies/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                MoviePutVM movie = await JsonSerializer.DeserializeAsync<MoviePutVM>(responseStream);
                return View(movie);
            }

            return RedirectOnError(response, "Edit", "edit an movie", "getting the movie details");
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MoviePutVM movie)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MMDB_API");

                var movieContent = new StringContent(JsonSerializer.Serialize(movie), Encoding.UTF8, "application/json");

                var response = await client.PutAsync("movies/" + id, movieContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectOnError(response, "Edit", "edit an movie", "saving the modified movie");
            }

            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("movies/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                MovieVM movie = await JsonSerializer.DeserializeAsync<MovieVM>(responseStream);
                return View(movie);
            }

            return RedirectOnError(response, "Delete", "delete a movie", "getting the movie details");
        }

        // POST: Movies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, MovieVM movie)
        {
            var client = _httpClientFactory.CreateClient("HolidayRequestsWebApi");

            var response = await client.DeleteAsync("movies/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectOnError(response, "Delete", "delete a movie", "deleting the movie");
        }
    }
}