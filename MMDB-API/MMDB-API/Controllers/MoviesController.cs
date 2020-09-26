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
    public class MoviesController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(MMDBContext context, IMovieRepository movieRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            return Ok(await _movieRepository.GetMovies().ConfigureAwait(false));
        }

        // GET: api/Movies
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetRecentMovies()
        {
            return Ok(await _movieRepository.GetRecentMovies().ConfigureAwait(false));
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetMovie(id).ConfigureAwait(false);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutMovie(int id, MoviePutDTO moviePutDTO)
        {
            if (moviePutDTO == null) { throw new ArgumentNullException(nameof(moviePutDTO)); }

            if (id != moviePutDTO.Id)
            {
                return BadRequest();
            }

            var movieResult = await _movieRepository.PutMovie(id, moviePutDTO).ConfigureAwait(false);

            if (movieResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<MovieDTO>> PostMovie(MoviePutDTO moviePostDTO)
        {
            var movieResult = await _movieRepository.PostMovie(moviePostDTO).ConfigureAwait(false);

            return CreatedAtAction("GetMovie", new { id = movieResult.Id }, movieResult);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<MovieDTO>> DeleteMovie(int id)
        {
            var movieResult = await _movieRepository.DeleteMovie(id).ConfigureAwait(false);

            if (movieResult == null)
            {
                return NotFound();
            }

            return movieResult;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
