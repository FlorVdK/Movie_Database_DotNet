using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MMDB_WebApp.Areas.Comment.Models;
using MMDB_WebApp.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace MMDB_WebApp.Areas.Comment.Controllers
{
    [Area("Comment")]
    public class CommentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CommentsController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Comment/Comments
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Comment/Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            return View();
        }

        // GET: Comment/Comments/Create
        public IActionResult Create(int id)
        {
            ViewData["MovieId"] = id;
            return View();
        }

        // POST: Comment/Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentPutVM comment)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MMDB_API");
                comment.Date = DateTime.Now;

                var commentContent = new StringContent(JsonSerializer.Serialize(comment), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("comments", commentContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Movies", new { id = comment.MovieId, area = "" });
                }

                return RedirectOnError(response, "Create", "create an comment", "creating an comment");
            }

            return View(comment);
        }

        // GET: Comment/Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: Comment/Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText,Date,UserId,UserName,UserAvatar,MovieId")] CommentVM commentVM)
        {
            return View();
        }

        // GET: Comment/Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: Comment/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectOnError(HttpResponseMessage response, string actionMethod, string forbiddenMessage, string errorMessage)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["RedirectController"] = "Comments";
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
