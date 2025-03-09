using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketsCinema.Models;
using TicketsCinema.Models.ViewModel;

namespace TicketsCinema.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorMovie>().HasKey(e => new { e.ActorId, e.MovieId });
        }
        public DbSet<TicketsCinema.Models.ViewModel.RegisterVM> RegisterVM { get; set; } = default!;
        public DbSet<TicketsCinema.Models.ViewModel.LoginVM> LoginVM { get; set; } = default!;

    }
}
