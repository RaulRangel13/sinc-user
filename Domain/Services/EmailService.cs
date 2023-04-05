using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(EmailModel emailModel)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string name = _configuration.GetValue<string>("SMTP:Name");
                string userName = _configuration.GetValue<string>("SMTP:UserName");
                string password = _configuration.GetValue<string>("SMTP:Password");
                int door = _configuration.GetValue<int>("SMTP:Door");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(userName, name);
                mail.To.Add(emailModel.Email);
                mail.Subject = emailModel.Subject;
                mail.Body = emailModel.Body;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, door))
                {
                    smtp.Credentials = new NetworkCredential(userName, password);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
