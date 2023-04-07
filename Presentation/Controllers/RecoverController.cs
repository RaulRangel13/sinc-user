using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services.Interfaces;

namespace Presentation.Controllers
{
    public class RecoverController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _accessor;

        public RecoverController(IUserService userService,
            IHttpContextAccessor accessor)
        {
            _userService = userService;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<IActionResult> RecoverPassword(string id)
        {
            try
            {
                if (_userService.IsLogged())
                    return RedirectToAction("Index", "Home");

                var customer = await _userService.AutorizationToken(id);
                if (!customer.Sucess)
                    return Unauthorized();

                ViewBag.Name = customer.Name;
                var model = new RecoverModel() { Token = id };
                return View(model);
            }
            catch (Exception e)
            {
                await HttpContext.SignOutAsync();
                ViewBag.msgError = "Erro ao tentar recuperar a senha, tente novamente mais tarde";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverModel model)
        {
            if (_userService.IsLogged())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var customer = await _userService.AutorizationToken(model.Token);
            if (!customer.Sucess)
                return Unauthorized();

            await _userService.ApiChangePasswordAsync(model);

            ViewBag.Name = customer.Name;
            ViewBag.reciverd = true;
            return View(model);
        }

        [HttpGet]
        public IActionResult RecoverEmailPassword()
        {
            if (_userService.IsLogged())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverEmailPassword(RecoverEmailPassModel model)
        {
            try
            {
                if (_userService.IsLogged())
                    return RedirectToAction("Index", "Home");

                if (!ModelState.IsValid)
                    return View(model);

                var baseUrl = _accessor.HttpContext.Request.Scheme + "://" +
                    _accessor.HttpContext.Request.Host.Value + Url.Action("RecoverPassword", "Recover");
                var recoverModel = new RecoverPasswordModel() { Email = model.Email, BaseUrl = baseUrl };
                var recoverEmailPassResponse = await _userService.ApiRecoverEmailPassAsync(recoverModel);
                if (!recoverEmailPassResponse.Sucess)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Email", recoverEmailPassResponse?.ErrorsMessage?.FirstOrDefault() ?? "Erro ao recuperar a senha");
                    return View(model);
                }

                @ViewBag.Email = model.Email;
                return View("RecoverSucess");
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
