using TicketsCinema.Data;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;
namespace TicketsCinema.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
