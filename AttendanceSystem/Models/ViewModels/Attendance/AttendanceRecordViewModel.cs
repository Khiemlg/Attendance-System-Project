using System;

namespace AttendanceSystem.ViewModels.Attendance
{
    public class AttendanceRecordViewModel
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Method { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Notes { get; set; }
    }
}
