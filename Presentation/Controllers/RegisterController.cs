using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services.Interfaces;

namespace Presentation.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService authService)
        {
            _userService = authService;
        }

        [HttpGet]
        public IActionResult SigUp()
        {
            if (_userService.IsLogged())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SigUp(RegisterModel model)
        {
            try
            {
                if (_userService.IsLogged())
                    return RedirectToAction("Index", "Home");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var loginResponse = await _userService.ApiRegisterAsync(model);
                if (!loginResponse.Sucess)
                {
                    ModelState.AddModelError("ConfirmPassword", loginResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao fazer o cadastro");
                    return View(model);
                }

                await _userService.FrontLoginAsync(HttpContext, loginResponse);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ViewBag.msgError = "Erro ao fazer o cadastro, tente novamente mais tarde";
                return View(model);
            }
        }
    }
}
