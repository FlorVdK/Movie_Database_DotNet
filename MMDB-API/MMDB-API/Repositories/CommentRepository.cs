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
    public class CommentRepository : ICommentRepository
    {
        private readonly MMDBContext _context;

        public CommentRepository(MMDBContext context)
        {
            _context = context;
        }
        public Task<CommentDTO> DeleteComment(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentDTO> GetComment(int id)
        {
            var comment = await _context.Comments
                .Select(i => new CommentDTO()
                {
                    Id = i.Id,
                    CommentText = i.CommentText,
                    Date = i.Date,
                    MovieId = i.MovieId,
                    UserId = i.UserId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            if (comment == null)
            {
                return null;
            }

            return comment;
        }

        public Task<IEnumerable<CommentDTO>> GetComments()
        {
            throw new NotImplementedException();
        }

        public async Task<CommentPutDTO> PostComment(CommentPutDTO commentPostDTO)
        {
            if (commentPostDTO == null) { throw new ArgumentNullException(nameof(commentPostDTO)); }

            var commentResult = _context.Comments.Add(new Comment()
            {
                CommentText = commentPostDTO.CommentText,
                Date = commentPostDTO.Date,
                MovieId = commentPostDTO.MovieId,
                UserId = commentPostDTO.UserId
            });

            await _context.SaveChangesAsync().ConfigureAwait(false);

            commentPostDTO.Id = commentResult.Entity.Id;

            return commentPostDTO;
        }

        public Task<CommentPutDTO> PutComment(int id, CommentPutDTO commentPutDTO)
        {
            throw new NotImplementedException();
        }
    }
}
