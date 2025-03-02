using Microsoft.AspNetCore.Mvc;
using TicketsCinema.Models;
using TicketsCinema.Repositories.IRepositories;

namespace TicketsCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = categoryRepository.Get();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                TempData["notifation"] = $"{category.Name} Added Successfully ";
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(filter: e => e.Id == categoryId);
            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category != null)
            {
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Edit");
        }

        public ActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);

            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();

                TempData["notifation"] = "Delete category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
