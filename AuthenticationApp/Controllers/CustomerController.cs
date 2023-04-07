using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
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
        private readonly ITokenService _tokenService;

        public CustomerController(ICustomerService customerService, 
            ITokenService tokenService)
        {
            _customerService = customerService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> SigUp([FromBody]CustomerSigUpRequestDto customerRequest)
        {
            var customerResponse = await _customerService.SaveNewCustomer(customerRequest);
            if(customerResponse.Sucess)
                return Ok(customerResponse);

            return BadRequest(customerResponse);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> SigIn([FromBody] CustomerSigInRequestDto customerRequest)
        {
            var customerLogin = await _customerService.LoginCustomer(customerRequest);
            if (customerLogin.Sucess)
                return Ok(customerLogin);

            return BadRequest(customerLogin);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> RecoverPassword(string email)
        {
            var customerResponse = await _customerService.RecoverPassword(email);
            if (customerResponse.Sucess)
                return Ok(customerResponse);

            return BadRequest(customerResponse);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> ChangePassword(CustomerPasswordRequestDto customerRequest)
        {
            var customerResponse = await _customerService.ChangePassword(customerRequest);
            if (customerResponse.Sucess)
                return Ok(customerResponse);

            return Unauthorized(customerResponse);
        }

        [HttpPost]
        public async Task<ActionResult<AuthResponse>> GenerateToken(CustomerSigInRequestDto customerRequest)
        {
            var customerEntity = new Customer() { Email = "raulrange@gmail.com", Id = 10 };
            var token = await _tokenService.GenerateTokenAsync(customerEntity);
            return Ok(token);
        }
    }
}
