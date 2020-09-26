using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface IActorRepository
    {
        Task<IEnumerable<ActorDTO>> GetActors();
        Task<ActorDTO> GetActor(int id);
        Task<ActorPutDTO> PostActor(ActorPutDTO actorPostDTO);
        Task<ActorPutDTO> PutActor(int id, ActorPutDTO actorPutDTO);
        Task<ActorDTO> DeleteActor(int id);
    }
}
