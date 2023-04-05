using AutoMapper;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public CustomerService(IMapper mapper,
            IConfiguration configuration,
            ICustomerRepository customerRepository,
            IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<bool> HasRegisteredCustomer(string email) =>
            await _customerRepository.HasRegisteredCustomer(email);

        public async Task<CustomerResponse> RecoverPassword(string email)
        {
            var customerEntity = await _customerRepository.GetByEmail(email);
            if (customerEntity is null)
                return ErrorResponse(email, "E-mail não encontrado");

            int passwordLength = 12;
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{};:,./<>?"; 

            Random random = new Random();
            string password = new string(Enumerable.Repeat(allowedChars, passwordLength).Select(s => s[random.Next(s.Length)]).ToArray());
            customerEntity.Password = password;
            HashPasswordGenerate(customerEntity);

            var emailModel = new EmailModel() { Body = $"sua nova senha é {password}", Email = email, Subject = "Recuperação de senha" };
            var sucess = _emailService.SendEmail(emailModel);
            if(!sucess)
                return ErrorResponse(email, "Erro ao enviar o e-mail");

            await _customerRepository.UpdateAsync(customerEntity);
            return new CustomerResponse() { Email = email};
        }

        public async Task<CustomerResponse> LoginCustomer(CustomerSigInRequestDto customer)
        {
            var customerEntity = await _customerRepository.GetByEmail(customer.Email);
            if (customerEntity is null)
                return ErrorResponse(customer.Email, "E-mail ou Senha inválido");

            if (!IsValidPassword(customerEntity, customer.Password))
                return ErrorResponse(customer.Email, "E-mail ou Senha inválido");

            return _mapper.Map<CustomerResponse>(customerEntity);
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
                    throw new InvalidOperationException("Erro ao verificar a senha");
            }
        }
        public async Task<CustomerResponse> SaveNewCustomer(CustomerSigUpRequestDto customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            if (HasRegisteredCustomer(customer.Email).Result)
            {
                var errorResponse = new CustomerResponse(false) { Email = customer.Email };
                errorResponse.AddErrors(new List<string>() { "E-mail ja cadastrado " });
                return errorResponse;
            }

            HashPasswordGenerate(customerEntity);
            customerEntity = await _customerRepository.CreateAsync(customerEntity);
            return _mapper.Map<CustomerResponse>(customerEntity);
        }

        private void HashPasswordGenerate(Customer customerEntity)
        {
            var passwordHasher = new PasswordHasher<Customer>();
            customerEntity.Password = passwordHasher.HashPassword(customerEntity, customerEntity.Password);
        }

        private CustomerResponse ErrorResponse(string email, string errorMessage)
        {
            var errorResponse = new CustomerResponse(false) { Email = email };
            errorResponse.AddErrors(new List<string>() { errorMessage });
            return errorResponse;
        }


    }
}
