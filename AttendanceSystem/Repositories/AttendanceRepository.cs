using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Attendance>> GetByUserAsync(int userId)
        {
            return await DbSet.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByClassSessionAsync(int sessionId)
        {
            return await DbSet.Where(a => a.ClassSessionId == sessionId).ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByEventAsync(int eventId)
        {
            return await DbSet.Where(a => a.EventId == eventId).ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
        {
            return await DbSet.Where(a => DbFunctions.TruncateTime(a.CheckInTime) == date.Date).ToListAsync();
        }
    }
}
