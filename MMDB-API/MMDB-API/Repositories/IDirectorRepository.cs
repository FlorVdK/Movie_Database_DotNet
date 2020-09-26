using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface IDirectorRepository
    {
        Task<IEnumerable<DirectorDTO>> GetDirectors();
        Task<DirectorDTO> GetDirector(int id);
        Task<DirectorPutDTO> PostDirector(DirectorPutDTO directorPostDTO);
        Task<DirectorPutDTO> PutDirector(int id, DirectorPutDTO directorPutDTO);
        Task<DirectorDTO> DeleteDirector(int id);
    }
}
