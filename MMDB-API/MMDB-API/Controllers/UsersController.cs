using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMDB_API.Data;
using MMDB_API.Dtos;
using MMDB_API.Models;
using MMDB_API.Repositories;
using MMDB_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkId=397860
//TODO add Roles
namespace MMDB_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MMDBContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UsersController(MMDBContext context, IUserRepository userRepository, IUserService userService)
        {
            _context = context;
            _userRepository = userRepository;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsers().ConfigureAwait(false));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserDetails(string id)
        {
            var user = await _userRepository.GetUserDetails(id).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserPostDTO>> PostUser(UserPostDTO userPostDTO)
        {
            var userResult = await _userRepository.PostUser(userPostDTO).ConfigureAwait(false);

            return CreatedAtAction("GetUserDetails", new { id = userResult.Id }, userResult);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserPutDTO userPutDTO)
        {
            if (userPutDTO == null) { throw new ArgumentNullException(nameof(userPutDTO)); }

            if (id != userPutDTO.Id)
            {
                return BadRequest();
            }

            var userResult = await _userRepository.PutUser(id, userPutDTO).ConfigureAwait(false);

            if (userResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH: api/Users/5/ChangePassword
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(string id, UserPatchDTO userPatchDTO)
        {
            if (userPatchDTO == null) { throw new ArgumentNullException(nameof(userPatchDTO)); }

            if (id != userPatchDTO.Id)
            {
                return BadRequest();
            }

            var userResult = await _userRepository.PatchUser(id, userPatchDTO).ConfigureAwait(false);

            if (userResult == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDeleteDTO>> DeleteUser(string id)
        {
            var userResult = await _userRepository.DeleteUser(id).ConfigureAwait(false);

            if (userResult == null)
            {
                return NotFound();
            }

            return userResult;
        }

        // POST: api/Users/login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDTO userLoginDTO)
        {
            var userResult = await _userService.Login(userLoginDTO.UserName, userLoginDTO.Password).ConfigureAwait(false);

            if (userResult == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return CreatedAtAction("GetUserDetails", new { id = userResult.Id }, userResult);
        }

        // POST: api/Users/register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO userRegisterDTO)
        {
            var userResult = await _userRepository.RegisterUser(userRegisterDTO).ConfigureAwait(false);

            if (userResult == null)
            {
                return BadRequest(new { message = "Registration failed" });
            }

            UserLoginDTO userLoginDTO = new UserLoginDTO
            {
                UserName = userRegisterDTO.Username,
                Password = userRegisterDTO.Password
            };

            return await Login(userLoginDTO).ConfigureAwait(false);
        }
    }
}
