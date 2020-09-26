using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMDB_API.Data;
using MMDB_API.Dtos;
using MMDB_API.Models;
using MMDB_API.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkId=397860

namespace MMDB_API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IGenreRepository _genrerepository;

        public GenresController(MMDBContext context, IGenreRepository genrerepository)
        {
            _context = context;
            _genrerepository = genrerepository;
        }

        // GET: api/Genres
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns>All genres</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            return Ok(await _genrerepository.GetGenres().ConfigureAwait(false));
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            var genre = await _genrerepository.GetGenre(id).ConfigureAwait(false);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutGenre(int id, GenrePutDTO genrePutDTO)
        {
            if (genrePutDTO == null) { throw new ArgumentNullException(nameof(genrePutDTO)); }

            if (id != genrePutDTO.Id)
            {
                return BadRequest();
            }

            var genreResult = await _genrerepository.PutGenre(id, genrePutDTO).ConfigureAwait(false);

            if (genreResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Genres
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenrePutDTO genrePutDTO)
        {
            var genreResult = await _genrerepository.PostGenre(genrePutDTO).ConfigureAwait(false);

            return CreatedAtAction("GetGenre", new { id = genreResult.Id }, genreResult);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GenreDTO>> DeleteGenre(int id)
        {
            var genreResult = await _genrerepository.DeleteGenre(id).ConfigureAwait(false);

            if (genreResult == null)
            {
                return NotFound();
            }

            return genreResult;
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
