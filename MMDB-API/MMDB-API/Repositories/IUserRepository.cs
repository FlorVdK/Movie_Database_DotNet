using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MMDB_API.Dtos;

namespace MMDB_API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserDetails(string id);
        Task<UserPostDTO> PostUser(UserPostDTO userPostDTO);
        Task<UserPutDTO> PutUser(string id, UserPutDTO userPutDTO);
        Task<UserPatchDTO> PatchUser(string id, UserPatchDTO userPatchDTO);
        Task<UserDeleteDTO> DeleteUser(string id);
        Task<UserRegisterDTO> RegisterUser(UserRegisterDTO userRegisterDTO);
    }
}
