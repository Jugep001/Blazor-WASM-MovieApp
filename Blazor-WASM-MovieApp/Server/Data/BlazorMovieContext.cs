using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blazor_WASM_MovieApp.Data
{
    public class BlazorMovieContext : IdentityDbContext<IdentityUser>
    {
        public BlazorMovieContext(DbContextOptions<BlazorMovieContext> options)
           : base(options)
        {

        }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Changelog> Logs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                        // Add Global filter to the Blog entity
                        .HasQueryFilter(p => p.IsDeleted == false);

        }
    }
}
