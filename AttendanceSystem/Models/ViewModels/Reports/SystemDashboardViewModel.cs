namespace AttendanceSystem.ViewModels.Reports
{
    public class SystemDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalEvents { get; set; }
        public int ActiveClasses { get; set; }
        public int ActiveEvents { get; set; }
        public int TotalCertificatesIssued { get; set; }
    }
}
