using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Models;
using Presentation.Services;
using Presentation.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Presentation.Controllers
{
    public class LoginController : Controller
    {
        private readonly Services.Interfaces.IAuthenticationService _authService;
        public LoginController(Services.Interfaces.IAuthenticationService apiAuthenticationService)
        {
            _authService = apiAuthenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> SigIn()
        {
            if(_authService.IsLogged())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SigIn(LoginModel model)
        {
            try
            {
                var loginResponse = await _authService.ApiLoginAsync(model);
                if (!loginResponse.Sucess)
                {
                    ModelState.AddModelError("Password", loginResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao fazer login");
                    return View(model);
                }

                await _authService.FrontLoginAsync(HttpContext, loginResponse);

                return RedirectToAction("Index","Home");
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ModelState.AddModelError("Password", "Erro ao fazer login, tente novamente mais tarde");
                return View(model);
            }
        }

        public async Task<IActionResult> SigOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
