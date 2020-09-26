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
    public class DirectorsController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IDirectorRepository _directorRepository;


        public DirectorsController(MMDBContext context, IDirectorRepository directorRepository)
        {
            _context = context;
            _directorRepository = directorRepository;
        }

        // GET: api/Directors
        /// <summary>
        /// Get all directors
        /// </summary>
        /// <returns>All directors</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirectors()
        {
            return Ok(await _directorRepository.GetDirectors().ConfigureAwait(false));
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorDTO>> GetDirector(int id)
        {
            var director = await _directorRepository.GetDirector(id).ConfigureAwait(false);

            if (director == null)
            {
                return NotFound();
            }

            return director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutDirector(int id, DirectorPutDTO directorPutDTO)
        {
            if (directorPutDTO == null) { throw new ArgumentNullException(nameof(directorPutDTO)); }

            if (id != directorPutDTO.Id)
            {
                return BadRequest();
            }

            var directorResult = await _directorRepository.PutDirector(id, directorPutDTO).ConfigureAwait(false);

            if (directorResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Directors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DirectorDTO>> PostDirector(DirectorPutDTO directorPutDTO)
        {
            var directorResult = await _directorRepository.PostDirector(directorPutDTO).ConfigureAwait(false);
            
            return CreatedAtAction("GetDirector", new { id = directorResult.Id }, directorResult);
        }

        // DELETE: api/Directors/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DirectorDTO>> DeleteDirector(int id)
        {
            var directorResult = await _directorRepository.DeleteDirector(id).ConfigureAwait(false);

            if (directorResult == null)
            {
                return NotFound();
            }

            return directorResult;
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
