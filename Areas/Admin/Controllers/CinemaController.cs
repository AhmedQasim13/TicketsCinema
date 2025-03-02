using Microsoft.AspNetCore.Mvc;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;

namespace TicketsCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        ICinemaRepository cinemaRepository;
        public CinemaController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get();
            return View(cinemas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile file)
        {
            ModelState.Remove("CinemaLogo");
            ModelState.Remove("file");

            #region Save CinemaLogo into wwwroot
            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemaLogo", fileName);

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Save img into db
                cinema.CinemaLogo = fileName;
            }
            #endregion
            cinemaRepository.Create(cinema);
            cinemaRepository.Commit();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int cinemaId)
        {

            var cinema = cinemaRepository.GetOne(filter: e => e.Id == cinemaId);
            return View(cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile file)
        {
            #region Save img into wwwroot
            var cinemaInDb = cinemaRepository.GetOne(e => e.Id == cinema.Id);

            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemaLogo", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemaLogo", cinemaInDb.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Save img into db
                cinema.CinemaLogo = fileName;
            }
            else
            {
                cinema.CinemaLogo = cinemaInDb.CinemaLogo;
            }
            #endregion

            if (cinema != null)
            {
                cinemaRepository.Edit(cinema);

                cinemaRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int cinemaId)
        {
            var cinema = cinemaRepository.GetOne(filter: e => e.Id == cinemaId);
            if (cinema != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemaLogo", cinema.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                cinemaRepository.Delete(cinema);
                cinemaRepository.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}
