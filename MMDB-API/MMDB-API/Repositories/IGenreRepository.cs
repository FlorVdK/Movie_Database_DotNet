using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<GenreDTO>> GetGenres();
        Task<GenreDTO> GetGenre(int id);
        Task<GenrePutDTO> PostGenre(GenrePutDTO genrePostDTO);
        Task<GenrePutDTO> PutGenre(int id, GenrePutDTO genrePutDTO);
        Task<GenreDTO> DeleteGenre(int id);
    }
}
