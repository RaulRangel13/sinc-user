using Domain.DTOs.Requests;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApp.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerRequestDto>>> GetAll([FromBody]CustomerRequestDto customerRequest)
        {
            var t = _customerService.CustomerRegister(customerRequest);
            return Ok();
        }
    }
}
