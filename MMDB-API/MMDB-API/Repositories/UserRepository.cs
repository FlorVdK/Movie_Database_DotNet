using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MMDB_API.Data;
using MMDB_API.Dtos;
using MMDB_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MMDBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly RoleManager<Role> _roleManager;

        public UserRepository(MMDBContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDeleteDTO> DeleteUser(string id)
        {
            var user = await _context.Users
                .Include(c => c.Comments)
                .Include(c => c.Votes)
                .FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);

            if (user == null)
            {
                return null;
            }

            _context.Comments.RemoveRange(user.Comments);
            _context.Votes.RemoveRange(user.Votes);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            await _userManager.DeleteAsync(user).ConfigureAwait(false);

            return new UserDeleteDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email
            };
        }

        public async Task<UserDTO> GetUserDetails(string id)
        {
            return await _context.Users.Include(u => u.UserRoles)
                .Select(u => new UserDTO()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Email = u.Email,
                    //Avatar = u.GetAvatar(),
                    Roles = u.UserRoles.Select(x => new UserRoleAssignmentDTO()
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description
                    }).ToList(),
                    Token = null
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return await _context.Users.Include(u => u.UserRoles)
                .Select(u => new UserDTO()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Email = u.Email,
                    //Avatar = u.GetAvatar(),
                    Roles = u.UserRoles.Select(x => new UserRoleAssignmentDTO()
                    {
                        Name = x.Role.Name,
                        Description = x.Role.Description
                    }).ToList(),
                    Token = null
                })
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<UserPatchDTO> PatchUser(string id, UserPatchDTO userPatchDTO)
        {
            if (userPatchDTO == null) { throw new ArgumentNullException(nameof(userPatchDTO)); }

            try
            {
                User user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
                _ = await _userManager.ChangePasswordAsync(user, userPatchDTO.CurrentPassword, userPatchDTO.NewPassword).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists(id).ConfigureAwait(false) == false)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return userPatchDTO;
        }

        public async Task<UserPostDTO> PostUser(UserPostDTO userPostDTO)
        {
            if (userPostDTO == null) { throw new ArgumentNullException(nameof(userPostDTO)); }

            User user = new User
            {
                FirstName = userPostDTO.FirstName,
                LastName = userPostDTO.LastName,
                UserName = userPostDTO.Username,
                Email = userPostDTO.Email
            };

            _ = await _userManager.CreateAsync(user, userPostDTO.Password).ConfigureAwait(false);

            userPostDTO.Id = user.Id;

            // Assign default user role to user
            await AssignRole(user).ConfigureAwait(false);

            return userPostDTO;
        }

        public async Task<UserPutDTO> PutUser(string id, UserPutDTO userPutDTO)
        {
            if (userPutDTO == null) { throw new ArgumentNullException(nameof(userPutDTO)); }

            try
            {
                User user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);

                user.FirstName = userPutDTO.FirstName;
                user.LastName = userPutDTO.LastName;
                user.Email = userPutDTO.Email;
                //user.SetAvatar(userPutDTO.Avatar);

                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                await _userManager.RemoveFromRolesAsync(user, roles.ToArray()).ConfigureAwait(false);

                await _userManager.AddToRolesAsync(user, userPutDTO.Roles).ConfigureAwait(false);

                await _userManager.UpdateAsync(user).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists(id).ConfigureAwait(false) == false)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return userPutDTO;
        }

        public async Task<UserRegisterDTO> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null) { throw new ArgumentNullException(nameof(userRegisterDTO)); }

            User user = new User
            {
                UserName = userRegisterDTO.Username,
                Email = userRegisterDTO.Email
            };

            var registerResult = await _userManager.CreateAsync(user, userRegisterDTO.Password).ConfigureAwait(false);

            if (registerResult.Succeeded)
            {
                userRegisterDTO.Id = user.Id;
                _context.Roles.Add(new Role { Name = "user", Description = "User" });
                _context.Roles.Add(new Role { Name = "admin", Description = "Admin" });
                _context.Roles.Add(new Role { Name = "superadmin", Description = "Superadmin" });

                // Assign default user role to user
                await AssignRole(user).ConfigureAwait(false);

                return userRegisterDTO;
            }

            return null;
        }

        private async Task<bool> UserExists(string id)
        {
            return await _userManager.FindByIdAsync(id).ConfigureAwait(false) != null ? true : false;
        }

        private async Task AssignRole(User user)
        {
            _ = await _userManager.AddToRoleAsync(user, "user").ConfigureAwait(false);

            return;
        }
    }
}
