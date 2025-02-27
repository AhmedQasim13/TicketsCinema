using TicketsCinema.Data;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;
namespace TicketsCinema.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
