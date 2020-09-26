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

namespace MMDB_WebApp.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DirectorsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            IEnumerable<DirectorVM> directors;

            var client = _httpClientFactory.CreateClient("MMDB_API");

            var response = await client.GetAsync("directors").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                directors = await JsonSerializer.DeserializeAsync<IEnumerable<DirectorVM>>(responseStream);
            }
            else
            {
                directors = Array.Empty<DirectorVM>();
            }

            return View(directors);
        }

        // GET: Directors/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Directors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Directors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Directors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Directors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Directors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}