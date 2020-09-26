using MMDB_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender gender { get; set; }
        private string Avatar { get; set; }

        //relationships
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
