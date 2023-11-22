using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using She_He_Store.Models;

namespace She_He_Store.Services
{
    public class EmailService : IMail
    {
        private readonly MailSettings _settings;
        public EmailService(IOptions<MailSettings> settings)
        {
            _settings= settings.Value;    
        }
        public void SendEmail(MailData mailData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailTo));
            message.Subject = mailData.EmailSubject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = mailData.EmailBody
            };

            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_settings.UserName, _settings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
    }
