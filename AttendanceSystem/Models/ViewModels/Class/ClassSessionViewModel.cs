using System;

namespace AttendanceSystem.ViewModels.Class
{
    public class ClassSessionViewModel
    {
        public int SessionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string QRCodeUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
