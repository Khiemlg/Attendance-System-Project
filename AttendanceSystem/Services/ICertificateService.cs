using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface ICertificateService
    {
        Task GenerateCertificateAsync(int eventId, int userId);
        Task<string> GetCertificatePathAsync(int certificateId);
    }
}
