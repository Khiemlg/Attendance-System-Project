using System;

namespace AttendanceSystem.ViewModels.Event
{
    public class EventFeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public string ParticipantName { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
