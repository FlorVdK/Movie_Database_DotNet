using MMDB_API.Dtos;
using MMDB_API.Helpers;
using MMDB_API.Models;
using MMDB_API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MMDB_API.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AppSettings> appSettings, SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<UserDTO> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            // return null if user not found
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false).ConfigureAwait(false);

            // return null if user failed to login
            if (!result.Succeeded)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var roleNames = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Id));

            foreach (string roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            UserDTO userDTO = await _userRepository.GetUserDetails(user.Id).ConfigureAwait(false);

            userDTO.Token = tokenHandler.WriteToken(token);

            return userDTO;
        }
    }
}