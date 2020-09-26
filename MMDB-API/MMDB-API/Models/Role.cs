using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMDB_API.Models
{
    public class Role : IdentityRole
    {
        [Required]
        public string Description { get; set; }

        // A Role can be assigned to many Users
        public ICollection<UserRoleAssignment> UserRoles { get; }
    }
}
