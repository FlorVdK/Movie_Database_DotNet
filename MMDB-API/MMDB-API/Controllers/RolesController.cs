using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MMDB_API.Data;
using MMDB_API.Dtos;
using MMDB_API.Repositories;

namespace MMDB_API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IRoleRepository _roleRepository;

        public RolesController(MMDBContext context, IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            return Ok(await _roleRepository.GetRoles().ConfigureAwait(false));
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(string id)
        {
            var role = await _roleRepository.GetRole(id).ConfigureAwait(false);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // POST: api/Roles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO)
        {
            var roleResult = await _roleRepository.PostRole(roleDTO).ConfigureAwait(false);

            return CreatedAtAction("GetRole", new { id = roleResult.Id }, roleResult);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, RoleDTO roleDTO)
        {
            if (roleDTO == null) { throw new ArgumentNullException(nameof(roleDTO)); }

            if (id != roleDTO.Id)
            {
                return BadRequest();
            }

            var roleResult = await _roleRepository.PutRole(id, roleDTO).ConfigureAwait(false);

            if (roleResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoleDTO>> DeleteRole(string id)
        {
            var roleResult = await _roleRepository.DeleteRole(id).ConfigureAwait(false);

            if (roleResult == null)
            {
                return NotFound();
            }

            return roleResult;
        }
    }
}