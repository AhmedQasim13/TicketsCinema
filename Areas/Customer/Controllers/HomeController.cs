using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;

namespace TicketsCinema.Area.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        IMovieRepository movieRepository;
        ICategoryRepository categoryRepository;
        ICinemaRepository cinemaRepository;
        IActorRepository actorRepository;
        IActorMovieRepository actorMovieRepository;
        public HomeController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository, IActorMovieRepository actorMovieRepository, IActorRepository actorRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
            this.actorRepository = actorRepository;
            this.actorMovieRepository = actorMovieRepository;
        }


        public IActionResult Index(string movieName)
        {

            var movies = movieRepository.Get(includes: [e => e.Category, c => c.Cinema]);
            if (movieName != null)
            {
                movies = movieRepository.Get(filter: e => e.Name.Contains(movieName), includes: [e => e.Category, c => c.Cinema]);
            }
            ViewBag.MovieName = movieName;
            return View(movies.ToList());

        }

        public IActionResult Details(int movieId)
        {
            var movieDetails = movieRepository.GetOne(
               filter: e => e.Id == movieId, includes: [e => e.Category, c => c.Cinema, e => e.ActorMovies]
                );
            var actorMovies = actorMovieRepository.Get(
                filter: e => e.MovieId == movieId,
                includes: [e => e.Actor]
                );
            ViewBag.ActorMovies = actorMovies;
            if (movieDetails != null)
            {
                return View(movieDetails);
            }
            return RedirectToAction("Index");
        }
        public IActionResult ActorDetails(int actorId)
        {
            var actorDetails = actorRepository.GetOne(
                e => e.Id == actorId,
                new Expression<Func<Actor, object>>[]
            {
                a => a.ActorMovies,
            });
            if (actorDetails != null)
            {
                return View(actorDetails);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
