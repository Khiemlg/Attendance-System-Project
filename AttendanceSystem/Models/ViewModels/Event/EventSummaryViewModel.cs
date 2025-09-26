using System;

namespace AttendanceSystem.ViewModels.Event
{
    public class EventSummaryViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int TotalParticipants { get; set; }
        public string DepartmentName { get; set; }
        public bool RequiresCertificate { get; set; }
    }
}
