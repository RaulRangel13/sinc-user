using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace AuthenticationApp.Controllers
{
    [Route("api/[controller]/[action]")]
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
            var customerResponse = await _customerService.SaveNewCustomerAsync(customerRequest);
            if(customerResponse.Sucess)
                return Ok(customerResponse);

            return BadRequest(customerResponse);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> SigIn([FromBody] CustomerSigInRequestDto customerRequest)
        {
            var customerLogin = await _customerService.LoginCustomerAsync(customerRequest);
            if (customerLogin.Sucess)
                return Ok(customerLogin);

            return BadRequest(customerLogin);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> RecoverPassword([FromBody] RecoverPasswordRequest recoverRequest)
        {
            string decodedString = WebUtility.UrlDecode(recoverRequest.BaseUrl);
            var customerResponse = await _customerService.RecoverPasswordAsync(recoverRequest.Email, decodedString);
            if (customerResponse.Sucess)
                return Ok(customerResponse);

            return BadRequest(customerResponse);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> ChangePassword(CustomerPasswordRequestDto customerRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var customerId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (customerId == null)
                return BadRequest();

            var customerResponse = await _customerService.ChangePasswordAsync(Convert.ToInt32(customerId), customerRequest.NewPassword);
            _customerService.SetToken(customerResponse, Request); 
            if (customerResponse.Sucess)
                return Ok(customerResponse);

            return Unauthorized(customerResponse);
        }


    }
}
