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
    public class ActorsController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IActorRepository _actorRepository;


        public ActorsController(MMDBContext context, IActorRepository actorRepository)
        {
            _context = context;
            _actorRepository = actorRepository;
        }

        // GET: api/Actors
        /// <summary>
        /// Get all Actors
        /// </summary>
        /// <returns>list of actors</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetActors()
        {
            return Ok(await _actorRepository.GetActors().ConfigureAwait(false));
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDTO>> GetActor(int id)
        {
            var actor = await _actorRepository.GetActor(id).ConfigureAwait(false);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        // PUT: api/Actors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutActor(int id, ActorPutDTO actorPutDTO)
        {
            if (actorPutDTO == null) { throw new ArgumentNullException(nameof(actorPutDTO)); }

            if (id != actorPutDTO.Id)
            {
                return BadRequest();
            }

            var actorResult = await _actorRepository.PutActor(id, actorPutDTO).ConfigureAwait(false);

            if (actorResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Actors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Actor>> PostActor(ActorPutDTO actorPutDTO)
        {
            var actorResult = await _actorRepository.PostActor(actorPutDTO).ConfigureAwait(false);

            return CreatedAtAction("GetActor", new { id = actorResult.Id }, actorResult);
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ActorDTO>> DeleteActor(int id)
        {
            var actorResult = await _actorRepository.DeleteActor(id).ConfigureAwait(false);

            if (actorResult == null)
            {
                return NotFound();
            }

            return actorResult;
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
