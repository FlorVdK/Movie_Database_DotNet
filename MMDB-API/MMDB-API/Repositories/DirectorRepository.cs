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
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MMDBContext _context;

        public DirectorRepository(MMDBContext context)
        {
            _context = context;
        }
        public async Task<DirectorDTO> DeleteDirector(int id)
        {
            var director = await _context.Directors.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            if (director == null)
            {
                return null;
            }

            _context.Directors.Remove(director);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new DirectorDTO()
            {
                Id = director.Id,
                Name = director.Name,
                gender = director.gender,
                DateOfBirth = director.DateOfBirth
            };
        }

        public async Task<IEnumerable<DirectorDTO>> GetDirectors()
        {
            return await _context.Directors
                .Select(i => new DirectorDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                    DateOfBirth = i.DateOfBirth,
                    Movies = i.Movies.Select(x => new DirectorMovieDTO()
                    {
                        Id = x.Id,
                        Title = x.Title
                    }).ToList()
                })
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<DirectorDTO> GetDirector(int id)
        {
            //var Director = await _context.Directors.FindAsync(id);
            var Director = await _context.Directors.Include(i => i.Movies)
                .Select(i => new DirectorDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                    DateOfBirth = i.DateOfBirth,
                    Movies = i.Movies.Select(x => new DirectorMovieDTO()
                    {
                        Id = x.Id,
                        Title = x.Title
                    }).ToList()
                })
                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);

            if (Director == null)
            {
                return null;
            }

            return Director;
        }

        public async Task<DirectorPutDTO> PostDirector(DirectorPutDTO directorsPostDTO)
        {
            if (directorsPostDTO == null) { throw new ArgumentNullException(nameof(directorsPostDTO)); }

            var directorResult = _context.Directors.Add(new Director()
            {
                Name = directorsPostDTO.Name,
                DateOfBirth = directorsPostDTO.DateOfBirth
            });

            await _context.SaveChangesAsync().ConfigureAwait(false);

            directorsPostDTO.Id = directorResult.Entity.Id;

            return directorsPostDTO;
        }

        public async Task<DirectorPutDTO> PutDirector(int id, DirectorPutDTO directorPutDTO)
        {
            if (directorPutDTO == null) { throw new ArgumentNullException(nameof(directorPutDTO)); }

            // DTO objects don't exist in the context class and won't be recognized by EF so the code below will not work!!!
            //_context.Entry(address).State = EntityState.Modified;

            try
            {
                Director director = await _context.Directors.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
                director.Name = directorPutDTO.Name;
                director.DateOfBirth = directorPutDTO.DateOfBirth;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return directorPutDTO;
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
