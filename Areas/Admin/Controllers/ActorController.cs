using Microsoft.AspNetCore.Mvc;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;

namespace TicketsCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        IActorRepository actorRepository;
        public ActorController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }
        public IActionResult Index()
        {
            var actors = actorRepository.Get();
            return View(actors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Actor());
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile file)
        {
            ModelState.Remove("ProfilePicture");
            ModelState.Remove("file");
            #region Save ProfilePicture into wwwroot
            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                // Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Save img into db
                actor.ProfilePicture = fileName;
            }
            #endregion

            actorRepository.Create(actor);
            actorRepository.Commit();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int actorId)
        {
            var actor = actorRepository.GetOne(filter: e => e.Id == actorId);
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile file)
        {
            #region Save img into wwwroot
            var actorInDb = actorRepository.GetOne(e => e.Id == actor.Id);

            if (file != null && file.Length > 0)
            {
                // File Name, File Path
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actorInDb.ProfilePicture);
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
                actor.ProfilePicture = fileName;
            }
            else
            {
                actor.ProfilePicture = actorInDb.ProfilePicture;
            }
            #endregion

            if (actor != null)
            {
                actorRepository.Edit(actor);

                actorRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult Delete(int actorId)
        {
            var actor = actorRepository.GetOne(filter: e => e.Id == actorId);
            if (actor != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemaLogo", actor.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                actorRepository.Delete(actor);
                actorRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
