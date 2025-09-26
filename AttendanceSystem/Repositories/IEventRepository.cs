using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> GetByCodeAsync(string code);
        Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status);
        Task<IEnumerable<Event>> SearchAsync(string keyword);
    }
}
