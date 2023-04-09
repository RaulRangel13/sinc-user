using AutoMapper;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public CustomerService(IMapper mapper,
            IConfiguration configuration,
            ICustomerRepository customerRepository,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<bool> HasRegisteredCustomerAsync(string email) =>
            await _customerRepository.HasRegisteredCustomerAsync(email);

        public async Task<CustomerResponse> LoginNoPasswordAsync(int id)
        {
            var customerEntity = await _customerRepository.GetByIdyAsync(id);
            if (customerEntity is null)
                return ErrorResponse(String.Empty, Resources.UserMessages.CustomerNotFoundError);

            var authResponse = await _tokenService.GenerateTokenAsync(customerEntity);
            var customerResponse = _mapper.Map<CustomerResponse>(customerEntity);
            customerResponse.Token = authResponse.Token;
            return customerResponse;
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(int id)
        {
            var customerEntity = await _customerRepository.GetByIdyAsync(id);
            if (customerEntity is null)
                return ErrorResponse(String.Empty, Resources.UserMessages.CustomerNotFoundError);

            return _mapper.Map<CustomerResponse>(customerEntity);
        }

        public void SetToken(CustomerResponse customerResponse, HttpRequest request)
        {
            var authorization = request.Headers[HeaderNames.Authorization];
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);
            customerResponse.Token = headerValue?.Parameter;
        }

        public async Task<CustomerResponse> RecoverPasswordAsync(string email, string baseuUrl)
        {
            var customerEntity = await _customerRepository.GetByEmailAsync(email);
            if (customerEntity is null)
                return ErrorResponse(email, Resources.UserMessages.EmailNotFoundError);

            var authResponse = await _tokenService.GenerateTokenAsync(customerEntity);
            var url = $"{baseuUrl}?id={authResponse.Token}";
            var emailModel = new EmailModel() { Body =String.Format(Resources.UserMessages.EmailBodyRecover, url), Email = email, Subject = Resources.UserMessages.EmailSubjectRecover };
            var sucess = _emailService.SendEmailAsync(emailModel);
            if(!sucess)
                return ErrorResponse(email, Resources.UserMessages.EmailSendError);

            return _mapper.Map<CustomerResponse>(customerEntity);
        }

        public async Task<CustomerResponse> LoginCustomerAsync(CustomerSigInRequestDto customer)
        {
            var customerEntity = await _customerRepository.GetByEmailAsync(customer.Email);
            if (customerEntity is null)
                return ErrorResponse(customer.Email, Resources.UserMessages.EmailPassInvalidError);

            if (!IsValidPassword(customerEntity, customer.Password))
                return ErrorResponse(customer.Email, Resources.UserMessages.EmailPassInvalidError);

            var customerResponse = _mapper.Map<CustomerResponse>(customerEntity);
            var authToken = await _tokenService.GenerateTokenAsync(customerEntity);
            customerResponse.Token = authToken.Token;
            return customerResponse;
        }

        public async Task<CustomerResponse> ChangePasswordAsync(int customerId, string password)
        {
            var customerEntity = await _customerRepository.GetByIdyAsync(customerId);
            if (customerEntity == null)
                return ErrorResponse(String.Empty, Resources.UserMessages.CustomerNotFoundError);

            customerEntity.Password = password;
            HashPasswordGenerate(customerEntity);
            _customerRepository.UpdateAsync(customerEntity).Wait();

            return _mapper.Map<CustomerResponse>(customerEntity);
        }

        public async Task<CustomerResponse> SaveNewCustomerAsync(CustomerSigUpRequestDto customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            if (HasRegisteredCustomerAsync(customer.Email).Result)
            {
                var errorResponse = new CustomerResponse(false) { Email = customer.Email };
                errorResponse.AddErrors(new List<string>() { Resources.UserMessages.AlreadyEamilError });
                return errorResponse;
            }

            HashPasswordGenerate(customerEntity);
            customerEntity = await _customerRepository.CreateAsync(customerEntity);
            var emailModel = new EmailModel() { Body = String.Format(Resources.UserMessages.EmailWlcomeBody, customer.Name), Email = customerEntity.Email, Subject = Resources.UserMessages.EmailSubjectWelcome };
            _emailService.SendEmailAsync(emailModel);
            return _mapper.Map<CustomerResponse>(customerEntity);
        }

        private void HashPasswordGenerate(Customer customerEntity)
        {
            var passwordHasher = new PasswordHasher<Customer>();
            customerEntity.Password = passwordHasher.HashPassword(customerEntity, customerEntity.Password);
        }

        private bool IsValidPassword(Customer customerEntity, string password)
        {
            var passwordHasher = new PasswordHasher<Customer>();
            var status = passwordHasher.VerifyHashedPassword(customerEntity, customerEntity.Password, password);
            switch (status)
            {
                case PasswordVerificationResult.Failed:
                    return false;
                case PasswordVerificationResult.Success:
                    return true;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    return true;
                default:
                    throw new InvalidOperationException(String.Format(Resources.UserMessages.GenericError, "verificar senha"));
            }
        }

        private CustomerResponse ErrorResponse(string email, string errorMessage)
        {
            var errorResponse = new CustomerResponse(false) { Email = email };
            errorResponse.AddErrors(new List<string>() { errorMessage });
            return errorResponse;
        }

    }
}
