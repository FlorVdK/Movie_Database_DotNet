using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieDTO>> GetMovies();
        Task<MovieDTO> GetMovie(int id);
        Task<MoviePutDTO> PostMovie(MoviePutDTO moviePostDTO);
        Task<MoviePutDTO> PutMovie(int id, MoviePutDTO moviePutDTO);
        Task<MovieDTO> DeleteMovie(int id);
        Task<IEnumerable<MovieDTO>> GetRecentMovies();
    }
}
