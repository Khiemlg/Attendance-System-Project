using System.Collections.Generic;

namespace AttendanceSystem.ViewModels.Attendance
{
    public class AttendanceRealtimeViewModel
    {
        public int EntityId { get; set; }
        public string EntityType { get; set; } // ClassSession hoặc Event
        public string Title { get; set; }
        public IEnumerable<AttendanceRecordViewModel> Records { get; set; }
    }
}
