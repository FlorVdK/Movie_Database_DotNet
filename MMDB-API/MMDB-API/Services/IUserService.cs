using MMDB_API.Dtos;
using MMDB_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Services
{
    public interface IUserService
    {
        Task<UserDTO> Login(string username, string password);
    }
}