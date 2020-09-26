using MMDB_API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MMDB_API.Repositories;
using MMDB_API.Models;

namespace MMDB_API.Helpers
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MMDBContext(serviceProvider.GetRequiredService<DbContextOptions<MMDBContext>>());
            var _userRepository = serviceProvider.GetRequiredService<IUserRepository>();

            // Look for any companies.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            Role superadminRole = new Role
            {
                Name = "superadmin",
                Description = "Superadministrator"
            };

            Role adminRole = new Role
            {
                Name = "admin",
                Description = "Administrator"
            };

            Role userRole = new Role
            {
                Name = "user",
                Description = "User"
            };

            context.Roles.AddRange(superadminRole, adminRole, userRole);
            context.SaveChanges();

        }
    }
}
