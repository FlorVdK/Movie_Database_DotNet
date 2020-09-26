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
    public class GenreRepository : IGenreRepository
    {
        private readonly MMDBContext _context;

        public GenreRepository(MMDBContext context)
        {
            _context = context;
        }

        public async Task<GenreDTO> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            if (genre == null)
            {
                return null;
            }

            _context.Genres.Remove(genre);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new GenreDTO()
            {
                Id = genre.Id,
                Category = genre.Category,
                Description = genre.Description
            };
        }

        public async Task<IEnumerable<GenreDTO>> GetGenres()
        {
            return await _context.Genres
                .Select(i => new GenreDTO()
                {
                    Id = i.Id,
                    Category = i.Category,
                    Description = i.Description,
                    Movies = i.MovieGenres.Select(x => new GenreMoviesDTO()
                    {
                        Id = x.MovieId,
                        Title = x.Movie.Title
                    }).ToList()
                })
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<GenreDTO> GetGenre(int id)
        {
            //var Genre = await _context.Genres.FindAsync(id);
            var Genre = await _context.Genres.Include(i => i.MovieGenres)
                .Select(i => new GenreDTO()
                {
                    Id = i.Id,
                    Category = i.Category,
                    Description = i.Description,
                    Movies = i.MovieGenres.Select(x => new GenreMoviesDTO()
                    {
                        Id = x.MovieId,
                        Title = x.Movie.Title
                    }).ToList()
                })
                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);

            if (Genre == null)
            {
                return null;
            }

            return Genre;
        }

        public async Task<GenrePutDTO> PostGenre(GenrePutDTO genrePostDTO)
        {
            if (genrePostDTO == null) { throw new ArgumentNullException(nameof(genrePostDTO)); }

            var genreResult = _context.Genres.Add(new Genre()
            {
                Category = genrePostDTO.Category,
                Description = genrePostDTO.Description
            });

            await _context.SaveChangesAsync().ConfigureAwait(false);

            genrePostDTO.Id = genreResult.Entity.Id;

            return genrePostDTO;
        }

        public async Task<GenrePutDTO> PutGenre(int id, GenrePutDTO genrePutDTO)
        {
            if (genrePutDTO == null) { throw new ArgumentNullException(nameof(genrePutDTO)); }
            

            try
            {
                Genre genre = await _context.Genres.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
                genre.Category = genrePutDTO.Category;
                genre.Description = genrePutDTO.Description;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return genrePutDTO;
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }

    }
}
