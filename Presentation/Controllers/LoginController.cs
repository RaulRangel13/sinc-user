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
        private readonly IUserService _userService;
        public LoginController(IUserService apiAuthenticationService)
        {
            _userService = apiAuthenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> SigIn()
        {
            if (_userService.IsLogged())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SigIn(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var loginResponse = await _userService.ApiLoginAsync(model);
                if (!loginResponse.Sucess)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Password", loginResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao fazer login");
                    return View(model);
                }

                var customerModel = new TwoFaModel()
                {
                    Email = loginResponse.Email,
                    Id = loginResponse.Id,
                    Name = loginResponse.Name
                };

                if (!_userService.ApiGenerateKeyAsync(customerModel.Id).Result)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Password", loginResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao fazer login");
                    return View(model);
                }

                return View("TwoFa", customerModel);
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ViewBag.msgError = "Erro ao fazer login, tente novamente mais tarde";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TwoFaGenerate(int id)
        {
            if (_userService.ApiGenerateKeyAsync(id).Result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFa(TwoFaModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                if (!_userService.ApiValidateKeyAsync(
                    new TwoFaValidateModel() { CustomerId = model.Id, Key = model.Key}).Result)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Key", "Código inválido");
                    return View(model);
                }

                var loginResponse = new UserResponse()
                {
                    Email = model.Email,
                    Id = model.Id,
                    Name = model.Name,
                    Sucess = true
                };
                await _userService.FrontLoginAsync(HttpContext, loginResponse);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ViewBag.msgError = "Erro ao fazer login, tente novamente mais tarde";
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
