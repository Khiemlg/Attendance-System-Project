using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Event> GetByCodeAsync(string code)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Code == code);
        }

        public async Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await DbSet.Where(e => e.StartDate >= startDate && e.EndDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status)
        {
            return await DbSet.Where(e => e.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Event>> SearchAsync(string keyword)
        {
            return await DbSet.Where(e => e.Name.Contains(keyword) || e.Code.Contains(keyword)).ToListAsync();
        }
    }
}
