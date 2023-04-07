using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;
using System.Diagnostics;

namespace Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = "CookieAuthentication")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthUserService _authUserService;

        public HomeController(ILogger<HomeController> logger, 
            AuthUserService authUserService)
        {
            _logger = logger;
            _authUserService = authUserService;
        }

        public IActionResult Index()
        {
            ViewBag.Name = _authUserService.Name;
            @ViewBag.Email = _authUserService.Email;
            return View();
        }
    }
}