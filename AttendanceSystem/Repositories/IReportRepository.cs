using AttendanceSystem.Models;
using AttendanceSystem.ViewModels.Reports;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IReportRepository
    {
        Task<AttendanceSummaryViewModel> GetClassAttendanceSummaryAsync(int classId);
        Task<EventSummaryViewModel> GetEventSummaryAsync(int eventId);
        Task<SystemDashboardViewModel> GetSystemDashboardAsync();
    }
}
