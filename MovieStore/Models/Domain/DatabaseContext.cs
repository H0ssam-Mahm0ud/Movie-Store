using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieStore.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<MovieGenre> MovieGenre { get; set; }

        public DbSet<Movie> Movies { get; set; }
    }
}
