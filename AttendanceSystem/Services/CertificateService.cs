using AttendanceSystem.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ApplicationDbContext _context;

        public CertificateService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task GenerateCertificateAsync(int eventId, int userId)
        {
            var certificate = _context.Certificates.FirstOrDefault(c => c.EventId == eventId && c.UserId == userId);
            if (certificate == null)
            {
                certificate = new Certificate
                {
                    EventId = eventId,
                    UserId = userId,
                    CertificateNumber = Guid.NewGuid().ToString("N").ToUpper(),
                    IssueDate = DateTime.Now
                };
            }

            var folder = Path.Combine("Uploads", "Certificates");
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, $"certificate-{certificate.CertificateNumber}.png");

            using (var bitmap = new Bitmap(800, 600))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font("Arial", 24))
            {
                graphics.Clear(Color.White);
                graphics.DrawString("Certificate of Participation", new Font("Arial", 32, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
                graphics.DrawString($"User ID: {userId}", font, Brushes.Black, new PointF(100, 200));
                graphics.DrawString($"Event ID: {eventId}", font, Brushes.Black, new PointF(100, 250));
                graphics.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), font, Brushes.Black, new PointF(100, 300));
                bitmap.Save(filePath, ImageFormat.Png);
            }

            certificate.FilePath = filePath;
            certificate.IsSent = false;
            certificate.SentDate = null;

            if (certificate.CertificateId == 0)
            {
                _context.Certificates.Add(certificate);
            }

            await _context.SaveChangesAsync();
        }

        public Task<string> GetCertificatePathAsync(int certificateId)
        {
            var path = _context.Certificates.Where(c => c.CertificateId == certificateId).Select(c => c.FilePath).FirstOrDefault();
            return Task.FromResult(path);
        }
    }
}
