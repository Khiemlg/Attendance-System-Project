using System.Collections.Generic;

namespace AttendanceSystem.ViewModels.Event
{
    public class EventDetailViewModel
    {
        public EventSummaryViewModel EventInfo { get; set; }
        public IEnumerable<EventParticipantViewModel> Participants { get; set; }
        public IEnumerable<EventSessionViewModel> Sessions { get; set; }
        public IEnumerable<EventFeedbackViewModel> Feedbacks { get; set; }
    }
}
