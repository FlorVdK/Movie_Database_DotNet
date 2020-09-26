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
    public class MovieRepository : IMovieRepository
    {
        private readonly MMDBContext _context;

        public MovieRepository(MMDBContext context)
        {
            _context = context;
        }

        public async Task<MovieDTO> DeleteMovie(int id)
        {
            var movie = await _context.Movies
                .Include(c => c.ActorMovies)
                .Include(c => c.Comments)
                .Include(c => c.Votes)
                .Include(c =>c.MovieGenres)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (movie == null)
            {
                return null;
            }

            _context.ActorMovies.RemoveRange(movie.ActorMovies);
            _context.Comments.RemoveRange(movie.Comments);
            _context.Votes.RemoveRange(movie.Votes);
            _context.MovieGenres.RemoveRange(movie.MovieGenres);

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new MovieDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DirectorId = movie.DirectorId,
                Poster = movie.Poster
            };
        }

        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            return await _context.Movies.Select(i => new MovieDTO()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                ReleaseDate = i.ReleaseDate,
                DirectorId = i.DirectorId,
                DirectorName = i.Director.Name,
                Poster = i.Poster,
                Actors = i.ActorMovies.Select(x => new ActorMovieDTO()
                {
                    Id = x.ActorId,
                    Name = x.Actor.Name,
                    Role = x.Role
                }).ToList(),
                Comments = i.Comments.Select(x => new CommentDTO()
                {
                    Id = x.Id,
                    CommentText = x.CommentText,
                    UserId = x.UserId,
                    Date = x.Date
                }).ToList(),
                Genres = i.MovieGenres.Select(x => new MovieGenresDTO()
                {
                    Id = x.GenreId,
                    Genre = x.Genre.Category
                }).ToList(),
                Votes = i.Votes.Select(x => new VotesDTO()
                {
                    Id = x.UserId,
                    Score = x.Score,
                    Title = x.Movie.Title
                }).ToList()

            }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<MovieDTO>> GetRecentMovies()
        {
            return await _context.Movies.Select(i => new MovieDTO()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                ReleaseDate = i.ReleaseDate,
                DirectorId = i.DirectorId,
                DirectorName = i.Director.Name,
                Poster = i.Poster
            }).Where(m =>m.ReleaseDate < DateTime.Now).OrderByDescending(m => m.ReleaseDate).Take(5).ToListAsync().ConfigureAwait(false);
        }

        public async Task<MovieDTO> GetMovie(int id)
        {
            var Movie = await _context.Movies.Select(i => new MovieDTO()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                ReleaseDate = i.ReleaseDate,
                DirectorId = i.DirectorId,
                DirectorName = i.Director.Name,
                Poster = i.Poster,
                Actors = i.ActorMovies.Select(x => new ActorMovieDTO()
                {
                    Id = x.ActorId,
                    Name = x.Actor.Name,
                    Role = x.Role
                }).ToList(),
                Comments = i.Comments.Select(x => new CommentDTO()
                {
                    Id = x.Id,
                    CommentText = x.CommentText,
                    UserName =x.User.UserName,
                    UserId = x.UserId,
                    Date = x.Date
                }).ToList(),
                Genres = i.MovieGenres.Select(x => new MovieGenresDTO()
                {
                    Id = x.GenreId,
                    Genre = x.Genre.Category
                }).ToList(),
                Votes = i.Votes.Select(x => new VotesDTO()
                {
                    Id = x.UserId,
                    Score = x.Score,
                    Title = x.Movie.Title
                }).ToList()

            }).FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);

            if (Movie == null)
            {
                return null;
            }

            return Movie;
        }

        public async Task<MoviePutDTO> PostMovie(MoviePutDTO moviePostDTO)
        {
            if (moviePostDTO == null) { throw new ArgumentNullException(nameof(moviePostDTO)); }

            var movieResult = _context.Movies.Add(new Movie()
            {
                Title = moviePostDTO.Title,
                Description = moviePostDTO.Description,
                ReleaseDate = moviePostDTO.ReleaseDate,
                DirectorId = moviePostDTO.DirectorId,
                Poster = moviePostDTO.Poster
            }) ;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            moviePostDTO.Id = movieResult.Entity.Id;

            return moviePostDTO;
        }

        public async Task<MoviePutDTO> PutMovie(int id, MoviePutDTO moviePutDTO)
        {
            if (moviePutDTO == null) { throw new ArgumentNullException(nameof(moviePutDTO)); }

            // DTO objects don't exist in the context class and won't be recognized by EF so the code below will not work!!!
            //_context.Entry(address).State = EntityState.Modified;

            try
            {
                Movie movie = await _context.Movies.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
                movie.Title = moviePutDTO.Title;
                movie.Description = moviePutDTO.Description;
                movie.ReleaseDate = moviePutDTO.ReleaseDate;
                movie.DirectorId = moviePutDTO.DirectorId;
                movie.Poster = moviePutDTO.Poster;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return moviePutDTO;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
