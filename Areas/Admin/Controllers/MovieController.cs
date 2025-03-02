using Microsoft.AspNetCore.Mvc;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;

namespace TicketsCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        ICategoryRepository categoryRepository;
        ICinemaRepository cinemaRepository;
        IMovieRepository movieRepository;
        public MovieController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var movies = movieRepository.Get();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var category = categoryRepository.Get();
            var cinema = cinemaRepository.Get();
            ViewBag.Category = category;
            ViewBag.Cinema = cinema;
            return View(new Movie());
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile file)
        {
            ModelState.Remove("ImgUrl");
            ModelState.Remove("file");

            #region Save ProfilePicture into wwwroot
            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save img into db
                movie.ImgUrl = fileName;
            }
            #endregion
            movieRepository.Create(movie);
            movieRepository.Commit();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int movieId)
        {
            var category = categoryRepository.Get();
            var cinema = cinemaRepository.Get();
            ViewBag.Category = category;
            ViewBag.Cinema = cinema;
            var movie = movieRepository.GetOne(filter: e => e.Id == movieId);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile file)
        {
            #region Save img into wwwroot
            var movieInDb = movieRepository.GetOne(e => e.Id == movie.Id);

            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movieInDb.ImgUrl);
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
                movie.ImgUrl = fileName;
            }
            else
            {
                movie.ImgUrl = movieInDb.ImgUrl;
            }
            #endregion

            if (movie != null)
            {
                movieRepository.Edit(movie);

                movieRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }




        public IActionResult Delete(int movieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == movieId);
            if (movie != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                movieRepository.Delete(movie);
                movieRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
