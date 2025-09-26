using System;

namespace AttendanceSystem.ViewModels.Reports
{
    public class EventSummaryViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalParticipants { get; set; }
        public int TotalCheckIns { get; set; }
        public int FeedbackCount { get; set; }
        public double AverageRating { get; set; }
    }
}
