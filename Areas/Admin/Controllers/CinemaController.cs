using Microsoft.AspNetCore.Mvc;

namespace TicketsCinema.Areas.Admin.Controllers
{
    public class CinemaController : Controller
    {
        [Area("Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
