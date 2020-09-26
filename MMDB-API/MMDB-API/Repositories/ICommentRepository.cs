using MMDB_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentDTO>> GetComments();
        Task<CommentDTO> GetComment(int id);
        Task<CommentPutDTO> PostComment(CommentPutDTO commentPostDTO);
        Task<CommentPutDTO> PutComment(int id, CommentPutDTO commentPutDTO);
        Task<CommentDTO> DeleteComment(int id);
    }
}
