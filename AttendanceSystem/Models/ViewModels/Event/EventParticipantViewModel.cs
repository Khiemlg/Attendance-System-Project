namespace AttendanceSystem.ViewModels.Event
{
    public class EventParticipantViewModel
    {
        public int ParticipantId { get; set; }
        public int UserId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
    }
}
