using TicketsCinema.Data;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;
namespace TicketsCinema.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
