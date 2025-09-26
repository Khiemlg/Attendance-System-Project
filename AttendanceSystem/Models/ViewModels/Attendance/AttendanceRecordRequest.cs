using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.ViewModels.Attendance
{
    public class AttendanceRecordRequest
    {
        [Required]
        public int UserId { get; set; }

        public int? ClassSessionId { get; set; }

        public int? EventId { get; set; }

        [Required]
        public string Method { get; set; } // QR hoặc Face

        public string Notes { get; set; }
    }
}
