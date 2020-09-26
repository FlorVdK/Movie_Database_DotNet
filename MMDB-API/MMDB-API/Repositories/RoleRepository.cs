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
    public class RoleRepository : IRoleRepository
    {
        private readonly MMDBContext _context;
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(MMDBContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<RoleDTO> DeleteRole(string id)
        {
            Role role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);

            if (role == null)
            {
                return null;
            }

            RoleDTO roleDTO = new RoleDTO()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            _ = await _roleManager.DeleteAsync(role).ConfigureAwait(false);

            return roleDTO;
        }

        public async Task<RoleDTO> GetRole(string id)
        {
            Role role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
            return new RoleDTO()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task<IEnumerable<RoleDTO>> GetRoles()
        {
            return await _roleManager.Roles
                .Select(r => new RoleDTO()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<RoleDTO> PostRole(RoleDTO roleDTO)
        {
            if (roleDTO == null) { throw new ArgumentNullException(nameof(roleDTO)); }

            Role role = new Role()
            {
                Name = roleDTO.Name,
                Description = roleDTO.Description
            };

            _ = await _roleManager.CreateAsync(role).ConfigureAwait(false);

            roleDTO.Id = role.Id;
            roleDTO.Name = role.Name;

            return roleDTO;
        }

        public async Task<RoleDTO> PutRole(string id, RoleDTO roleDTO)
        {

            if (roleDTO == null) { throw new ArgumentNullException(nameof(roleDTO)); }

            try
            {
                Role role = await _roleManager.FindByIdAsync(roleDTO.Id).ConfigureAwait(false);
                role.Name =  roleDTO.Name;
                role.Description = roleDTO.Description;
                _ = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await RoleExists(roleDTO.Name).ConfigureAwait(false)))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return roleDTO;
        }

        private async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName).ConfigureAwait(false);
        }
    }
}
