using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MMDB_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MMDB_API.Data
{
    public class MMDBContext : IdentityDbContext
        <
            User,
            IdentityRole,
            string,
            IdentityUserClaim<string>,
            IdentityUserRole<string>,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
        >
    {
        public MMDBContext(DbContextOptions<MMDBContext> options)
            : base(options)
        {
        }

        public DbSet<MMDB_API.Models.Actor> Actors { get; set; }
        public DbSet<MMDB_API.Models.ActorMovie> ActorMovies { get; set; }
        public DbSet<MMDB_API.Models.Comment> Comments { get; set; }
        public DbSet<MMDB_API.Models.Director> Directors { get; set; }
        public DbSet<MMDB_API.Models.Genre> Genres { get; set; }
        public DbSet<MMDB_API.Models.Movie> Movies { get; set; }
        public DbSet<MMDB_API.Models.MovieGenre> MovieGenres { get; set; }
        public DbSet<MMDB_API.Models.Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (modelBuilder == null) { throw new ArgumentNullException(nameof(modelBuilder)); }
            // Fluent API Configuration
            // ========================

            // Director Movie relations
            // ========================

            modelBuilder.Entity<Movie>()
                .HasOne(c => c.Director)
                .WithMany(a => a.Movies)
                .HasForeignKey(c => c.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Actor Movie relations
            // ========================

            modelBuilder.Entity<ActorMovie>()
                .HasKey(u => new { u.ActorId, u.MovieId }); // Composite Primary Key

            modelBuilder.Entity<ActorMovie>()
                .HasOne(u => u.Actor)
                .WithMany(u => u.Movies)
                .HasForeignKey(u => u.ActorId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding User may not be deleted

            modelBuilder.Entity<ActorMovie>()
                .HasOne(u => u.Movie)
                .WithMany(r => r.ActorMovies)
                .HasForeignKey(u => u.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding Role may not be deleted

            // Genre Movie relations
            // ========================

            modelBuilder.Entity<MovieGenre>()
                .HasKey(u => new { u.MovieId, u.GenreId }); // Composite Primary Key

            modelBuilder.Entity<MovieGenre>()
                .HasOne(u => u.Movie)
                .WithMany(u => u.MovieGenres)
                .HasForeignKey(u => u.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding User may not be deleted

            modelBuilder.Entity<MovieGenre>()
                .HasOne(u => u.Genre)
                .WithMany(r => r.MovieGenres)
                .HasForeignKey(u => u.GenreId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding Role may not be deleted

            // User Movie relations
            // ========================

            //Vote

            modelBuilder.Entity<Vote>()
                .HasKey(u => new { u.MovieId, u.UserId }); // Composite Primary Key

            modelBuilder.Entity<Vote>()
                .HasOne(u => u.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding User may not be deleted

            modelBuilder.Entity<Vote>()
                .HasOne(u => u.Movie)
                .WithMany(r => r.Votes)
                .HasForeignKey(u => u.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding Role may not be deleted

            //Comments

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding User may not be deleted

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.Movie)
                .WithMany(r => r.Comments)
                .HasForeignKey(u => u.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // When a UserRoleAssignment is deleted, the corresponding Role may not be deleted

        }
    }
}
