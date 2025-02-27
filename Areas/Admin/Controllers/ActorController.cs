using Microsoft.AspNetCore.Mvc;

namespace TicketsCinema.Areas.Admin.Controllers
{
    public class ActorController : Controller
    {
        [Area("Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
