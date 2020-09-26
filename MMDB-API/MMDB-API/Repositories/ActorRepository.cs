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
    public class ActorRepository : IActorRepository
    {
        private readonly MMDBContext _context;

        public ActorRepository(MMDBContext context)
        {
            _context = context;
        }

        public async Task<ActorDTO> DeleteActor(int id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            if (actor == null)
            {
                return null;
            }

            _context.Actors.Remove(actor);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new ActorDTO()
            {
                Id = actor.Id,
                DateOfBirth = actor.DateOfBirth,
                gender = actor.gender
            };
        }

        public async Task<IEnumerable<ActorDTO>> GetActors()
        {
            return await _context.Actors
                .Select(i => new ActorDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                    DateOfBirth = i.DateOfBirth,
                    gender = i.gender,
                    Avatar = i.Avatar,
                    Movies = i.Movies.Select(x => new ActorMovieDTO
                    {
                        Id = x.MovieId,
                        Name = x.Movie.Title,
                        Role = x.Role
                    }).ToList()
                })
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<ActorDTO> GetActor(int id)
        {
            var Actor = await _context.Actors
                .Select(i => new ActorDTO()
                {
                    Id = i.Id,
                    Name = i.Name,
                    DateOfBirth = i.DateOfBirth,
                    gender = i.gender,
                    Avatar = i.Avatar,
                    Movies = i.Movies.Select(x => new ActorMovieDTO
                    {
                        Id = x.MovieId,
                        Name = x.Movie.Title,
                        Role = x.Role,
                        image = x.Movie.Poster
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            if (Actor == null)
            {
                return null;
            }

            return Actor;
        }

        public async Task<ActorPutDTO> PostActor(ActorPutDTO actorPostDTO)
        {
            if (actorPostDTO == null) { throw new ArgumentNullException(nameof(actorPostDTO)); }

            var actorResult = _context.Actors.Add(new Actor()
            {
                Name = actorPostDTO.Name,
                DateOfBirth = actorPostDTO.DateOfBirth,
                gender = actorPostDTO.gender,
                Avatar = actorPostDTO.Avatar
            });

            await _context.SaveChangesAsync().ConfigureAwait(false);

            actorPostDTO.Id = actorResult.Entity.Id;

            return actorPostDTO;
        }

        public async Task<ActorPutDTO> PutActor(int id, ActorPutDTO actorPutDTO)
        {

            if (actorPutDTO == null) { throw new ArgumentNullException(nameof(actorPutDTO)); }

            try
            {
                Actor actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
                actor.Name = actorPutDTO.Name;
                actor.DateOfBirth = actorPutDTO.DateOfBirth;
                actor.gender = actorPutDTO.gender;
                actor.Avatar = actorPutDTO.Avatar;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return actorPutDTO;
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
