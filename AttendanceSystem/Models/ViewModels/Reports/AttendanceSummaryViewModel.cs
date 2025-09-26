using System;

namespace AttendanceSystem.ViewModels.Reports
{
    public class AttendanceSummaryViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int TotalSessions { get; set; }
        public int TotalStudents { get; set; }
        public int TotalAttendances { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateCount { get; set; }
        public int ExcusedCount { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
    }
}
