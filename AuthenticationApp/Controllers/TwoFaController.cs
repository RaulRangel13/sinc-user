using Domain.DTOs.Requests;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TwoFaController : Controller
    {
        private readonly ITwoFAService _wofaService;

        public TwoFaController(ITwoFAService wofaService)
        {
            _wofaService = wofaService;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateKey([FromBody] TwoFaRequestDto twoFaRequest)
        {
            var isValidKey = await _wofaService.ValidateKeyAsync(twoFaRequest.Key, twoFaRequest.CustomerId);
            if (isValidKey)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GenerateKey(int id)
        {
            var twoFa = await _wofaService.GenerateKeyAsync(id);
            if(twoFa is null)
                return BadRequest();

            return Ok();
        }
    }
}
