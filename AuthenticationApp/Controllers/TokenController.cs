using AutoMapper;
using Azure;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace AuthenticationApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public TokenController(ITokenService tokenService,
            ICustomerService customerService,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<AuthResponse>> GenerateToken(CustomerSigInRequestDto customerRequest)
        {
            var customerResponse = await _customerService.LoginCustomerAsync(customerRequest);
            if (!customerResponse.Sucess)
                BadRequest(customerResponse);

            var customerEntity = _mapper.Map<Customer>(customerResponse);
            var token = await _tokenService.GenerateTokenAsync(customerEntity);
            return Ok(token);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerResponse>> RefreshLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var customerId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (customerId == null)
                return BadRequest();

            var resultado = await _customerService.LoginNoPasswordAsync(Convert.ToInt32(customerId));
            if (resultado.Sucess)
                return Ok(resultado);

            return Unauthorized(resultado);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CustomerResponse>> AutorizationToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var customerId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (customerId == null)
                return BadRequest();

            var response = await _customerService.GetCustomerByIdAsync(Convert.ToInt32(customerId));
            _customerService.SetToken(response, Request);

            return Ok(response);
        }
    }
}
