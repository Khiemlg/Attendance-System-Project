using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetByUserAsync(int userId);
        Task<IEnumerable<Attendance>> GetByClassSessionAsync(int sessionId);
        Task<IEnumerable<Attendance>> GetByEventAsync(int eventId);
        Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
    }
}
