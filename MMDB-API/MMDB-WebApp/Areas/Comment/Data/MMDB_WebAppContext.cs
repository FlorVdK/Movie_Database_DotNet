using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MMDB_WebApp.Areas.Comment.Models;

namespace MMDB_WebApp.Data
{
    public class MMDB_WebAppContext : DbContext
    {
        public MMDB_WebAppContext (DbContextOptions<MMDB_WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<MMDB_WebApp.Areas.Comment.Models.CommentVM> CommentVM { get; set; }
    }
}
