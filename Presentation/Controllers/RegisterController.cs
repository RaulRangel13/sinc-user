using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services.Interfaces;

namespace Presentation.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _authService;

        public RegisterController(IUserService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult SigUp()
        {
            if (_authService.IsLogged())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SigUp(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var loginResponse = await _authService.ApiRegisterAsync(model);
                if (!loginResponse.Sucess)
                {
                    ModelState.AddModelError("ConfirmPassword", loginResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao fazer login");
                    return View(model);
                }

                await _authService.FrontLoginAsync(HttpContext, loginResponse);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ViewBag.msgError = "Erro ao fazer login, tente novamente mais tarde";
                return View(model);
            }
        }
    }
}
