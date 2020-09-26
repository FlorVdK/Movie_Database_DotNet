using MMDB_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender gender { get; set; }
        public string Avatar { get; set; }

        //Relationships
        public virtual ICollection<ActorMovie> Movies { get; set; } 
    }
}
