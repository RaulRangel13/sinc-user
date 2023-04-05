using AutoMapper;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<bool> HasRegisteredCustomer(string email) =>
            await _customerRepository.HasRegisteredCustomer(email);

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
                    throw new InvalidOperationException("Erro ai verificar a senha");
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
            errorResponse.AddErrors(new List<string>() { "E-mail ou Senha inválido" });
            return errorResponse;
        }
    }
}
