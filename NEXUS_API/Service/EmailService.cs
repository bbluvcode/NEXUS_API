using Microsoft.Extensions.Options;
using NEXUS_API.Helpers;
using System.Net.Mail;
using System.Net;

namespace NEXUS_API.Service
{
    public class EmailService
    {
        private readonly EmailSetting _emailSettings;
        public EmailService(IOptions<EmailSetting> emailSettings, IWebHostEnvironment env)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendMailAsync(EmailRequest emailRequest)
        {
            var fromAddress = new MailAddress(_emailSettings.UserName);
            var toAddress = new MailAddress(emailRequest.ToMail);
            var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = _emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailRequest.Subject,
                Body = emailRequest.HtmlContent,
                IsBodyHtml = false
            };
            await smtp.SendMailAsync(message);
        }
    }
}
