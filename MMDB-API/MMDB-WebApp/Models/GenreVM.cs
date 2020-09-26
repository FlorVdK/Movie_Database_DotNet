using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class GenreVM
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        //Relationships
        public virtual ICollection<GenreMoviesVM> Movies { get; set; }
    }
}
