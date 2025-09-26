using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService()
        {
            _apiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
            _fromEmail = ConfigurationManager.AppSettings["FromEmail"];
            _fromName = ConfigurationManager.AppSettings["FromName"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string attachmentPath = null)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("SendGrid API key is not configured.");
            }

            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);

            if (!string.IsNullOrEmpty(attachmentPath))
            {
                var bytes = System.IO.File.ReadAllBytes(attachmentPath);
                var fileName = System.IO.Path.GetFileName(attachmentPath);
                msg.AddAttachment(fileName, Convert.ToBase64String(bytes));
            }

            await client.SendEmailAsync(msg);
        }
    }
}
