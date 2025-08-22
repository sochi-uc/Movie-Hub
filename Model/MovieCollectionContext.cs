using Microsoft.EntityFrameworkCore;

namespace Movie_Hub.Model
{ //DBconext
    public class MovieCollectionContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public MovieCollectionContext(DbContextOptions<MovieCollectionContext> options) : base(options)
        {

        }

    }
}
