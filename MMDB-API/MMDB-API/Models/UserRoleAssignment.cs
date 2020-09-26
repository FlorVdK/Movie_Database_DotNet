using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class UserRoleAssignment : IdentityUserRole<string>
    {
        public User User { get; set; } // Navigation property
        public Role Role { get; set; } // Navigation property
    }
}
