using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_WebApp.Models
{
    public class RoleVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //relationships
        public virtual ICollection<UserVM> Users { get; set; }
    }
}
