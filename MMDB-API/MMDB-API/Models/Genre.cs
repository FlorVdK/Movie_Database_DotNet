using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        //Relationships
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
