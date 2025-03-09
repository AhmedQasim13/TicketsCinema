using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsCinema.Models;
using TicketsCinema.Models.ViewModel;

namespace TicketsCinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    FullName = registerVM.Fullname,
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                };
                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerVM);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var mailresult = await userManager.FindByEmailAsync(loginVM.Email);
                if (mailresult != null)
                {
                    var passresult = await userManager.CheckPasswordAsync(mailresult, loginVM.Password);
                    if (passresult)
                    {
                        //login
                        await signInManager.SignInAsync(mailresult, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            return View(loginVM);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

    }
}
