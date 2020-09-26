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

namespace MMDB_WebApp.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ActorsController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            IEnumerable<ActorVM> actors;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("actors").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                actors = await JsonSerializer.DeserializeAsync<IEnumerable<ActorVM>>(responseStream);
            }
            else
            {
                actors = Array.Empty<ActorVM>();
            }

            return View(actors);
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ActorVM actor;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("actors/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                actor = await JsonSerializer.DeserializeAsync<ActorVM>(responseStream);
            }
            else
            {
                actor = null;
            }

            return View(actor);
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorVM actor)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MMDB_API");

                string uniqueFileName = UploadedFile(actor);

                ActorPutVM actorPost = new ActorPutVM
                {
                    Name = actor.Name,
                    DateOfBirth = actor.DateOfBirth,
                    gender = actor.gender,
                    Avatar = uniqueFileName
                };

                var actorContent = new StringContent(JsonSerializer.Serialize(actorPost), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("actors", actorContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectOnError(response, "Create", "create an actor", "creating an actor");
            }

            return View(actor);
        }

        private string UploadedFile(ActorVM actor)
        {
            string uniqueFileName = null;

            if (actor.AvatarFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Avatars");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + actor.AvatarFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    actor.AvatarFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Actors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActorPutVM actor)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MMDB_API");

                var actorContent = new StringContent(JsonSerializer.Serialize(actor), Encoding.UTF8, "application/json");

                var response = await client.PutAsync("actors/" + id, actorContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectOnError(response, "Edit", "edit an actor", "saving the modified actor");
            }

            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("actors/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                ActorVM actor = await JsonSerializer.DeserializeAsync<ActorVM>(responseStream);
                return View(actor);
            }

            return RedirectOnError(response, "Delete", "delete an actor", "getting the actor details");
        }

        // POST: Actors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("actors/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                ActorVM actor = await JsonSerializer.DeserializeAsync<ActorVM>(responseStream);
                return View(actor);
            }

            return RedirectOnError(response, "Delete", "delete an actor", "getting the actor details");
        }

        private IActionResult RedirectOnError(HttpResponseMessage response, string actionMethod, string forbiddenMessage, string errorMessage)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["RedirectController"] = "Actors";
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
    }
}