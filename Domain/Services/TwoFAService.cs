using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class TwoFAService : ITwoFAService
    {
        private readonly ITwoFaRepository _twoFaRepository;
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;

        public TwoFAService(ITwoFaRepository twoFaRepository,
            IEmailService emailService,
            ICustomerService customerService)
        {
            _twoFaRepository = twoFaRepository;
            _emailService = emailService;
            _customerService = customerService;
        }

        public async Task<TwoFA> GenerateKeyAsync(int customerId)
        {
            var twoFa = await _twoFaRepository.GetByCustomer(customerId);
            if (twoFa is null)
            {
                var twoFaEntity = new TwoFA()
                {
                    CustomerId = customerId,
                    key = new Random().Next(9999).ToString(),
                    CreatedAt = DateTime.Now,
                    ExpirtionDate = DateTime.Now.AddMinutes(3),
                    AlteratedAt = DateTime.Now
                };

                twoFa = await _twoFaRepository.CreateAsync(twoFaEntity);
            }
            else
            {
                twoFa.key = new Random().Next(9999).ToString();
                twoFa.AlteratedAt = DateTime.Now;
                twoFa.ExpirtionDate = DateTime.Now.AddMinutes(3);
                twoFa = await _twoFaRepository.UpdateAsync(twoFa);
            }

            var customerEntity = await _customerService.GetCustomerByIdAsync(customerId);
            var emailModel = new EmailModel() { Body = $"olá {customerEntity.Name}, seu código de verificação é {twoFa.key}.", Email = customerEntity.Email, Subject = "Código de verificação" };
            _emailService.SendEmail(emailModel);

            return twoFa;
        }

        public async Task<bool> ValidateKeyAsync(string key, int customerId)
        {
            var twoFa = await _twoFaRepository.GetCustomerKey(key, customerId);
            if (twoFa is null)
                return false;

            if (twoFa.ExpirtionDate < DateTime.Now)
                return false;

            return true;
        }
    }
}
