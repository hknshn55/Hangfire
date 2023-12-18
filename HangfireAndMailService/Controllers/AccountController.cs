using Hangfire;
using HangfireAndMailService.Models.ViewModels;
using HangfireAndMailService.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangfireAndMailService.Controllers
{
    public class AccountController : Controller
    {
        IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
          var res = await _authService.AddAsync(registerVM);
          return res.Succeeded ? RedirectToAction("Index","Home"):View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM )
        {
            var res = await _authService.LoginAsync(loginVM);
            return res.Succeeded ? RedirectToAction("Index", "Home") : View();
        }
    }
}
