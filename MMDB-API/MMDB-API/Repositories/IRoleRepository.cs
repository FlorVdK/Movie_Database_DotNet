using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDTO>> GetRoles();
        Task<RoleDTO> GetRole(string id);
        Task<RoleDTO> PostRole(RoleDTO roleDTO);
        Task<RoleDTO> PutRole(string id, RoleDTO roleDTO);
        Task<RoleDTO> DeleteRole(string id);
    }
}
