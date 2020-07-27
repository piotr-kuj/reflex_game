using com.sun.corba.se.pept.protocol;
using KIK.Options;
using Microsoft.Extensions.Options;
using sun.net.smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace KIK.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmail(string emailTo, string subject, string msg)
        {
            using (var client = new System.Net.Mail.SmtpClient(_emailServiceOptions.MailServer, int.Parse(_emailServiceOptions.MailPort)))
            { 
                if(bool.Parse(_emailServiceOptions.UseSSL)==true)
                {
                    client.EnableSsl = true;
                }

                if(!string.IsNullOrEmpty(_emailServiceOptions.UserId))
                {
                    client.Credentials = new NetworkCredential(_emailServiceOptions.UserId, _emailServiceOptions.Password);
                }
                client.Send(new MailMessage("example@example.com", emailTo, subject, msg));
            }

            return Task.CompletedTask;
        }

        private EmailServiceOptions _emailServiceOptions;
        public EmailService(IOptions<EmailServiceOptions> emailServiceOptions)
        {
            _emailServiceOptions = emailServiceOptions.Value;
        }


    }
}
