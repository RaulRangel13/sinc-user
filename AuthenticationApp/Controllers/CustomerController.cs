using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApp.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> SignUp([FromBody]CustomerSigUpRequestDto customerRequest)
        {
            var customerResponse = await _customerService.SaveNewCustomer(customerRequest);
            if(!customerResponse.Sucess)
                return BadRequest(customerResponse);

            return Ok(customerResponse);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> SignIn([FromBody] CustomerSigInRequestDto customerRequest)
        {
            var customerLogin = await _customerService.LoginCustomer(customerRequest);
            if (!customerLogin.Sucess)
                return BadRequest(customerLogin);

            return Ok(customerLogin);
        }
    }
}
