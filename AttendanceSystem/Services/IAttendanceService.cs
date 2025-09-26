using AttendanceSystem.ViewModels.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IAttendanceService
    {
        Task RecordAttendanceAsync(AttendanceRecordRequest request);
        Task<IEnumerable<AttendanceRecordViewModel>> GetAttendanceBySessionAsync(int sessionId);
        Task<IEnumerable<AttendanceRecordViewModel>> GetAttendanceByEventAsync(int eventId);
        Task<AttendanceRealtimeViewModel> GetRealtimeAttendanceAsync(int entityId, string entityType);
    }
}
