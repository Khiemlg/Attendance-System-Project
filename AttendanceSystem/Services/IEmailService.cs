using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent, string attachmentPath = null);
    }
}
