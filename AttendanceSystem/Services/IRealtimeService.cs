using AttendanceSystem.ViewModels.Attendance;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IRealtimeService
    {
        Task BroadcastAttendanceAsync(AttendanceRealtimeViewModel model);
    }
}
