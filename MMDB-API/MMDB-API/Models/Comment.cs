using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime Date { get; set; }

        //Relationships
        public string UserId { get; set; }
        public User User { get; set; }
        public int ? MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
