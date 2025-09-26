using AttendanceSystem.ViewModels.Attendance;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class RealtimeService : IRealtimeService
    {
        public Task BroadcastAttendanceAsync(AttendanceRealtimeViewModel model)
        {
            // TODO: Integrate SignalR or WebSocket broadcasting here
            return Task.CompletedTask;
        }
    }
}
