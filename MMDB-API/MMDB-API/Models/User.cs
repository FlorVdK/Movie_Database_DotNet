using Microsoft.AspNetCore.Identity;
using MMDB_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender gender { get; set; }
        private string Avatar { get; set; }

        //Relationships
        public ICollection<UserRoleAssignment> UserRoles { get; }
        public virtual ICollection<Comment> Comments { get; }
        public virtual ICollection<Vote> Votes { get; }
    }
}
