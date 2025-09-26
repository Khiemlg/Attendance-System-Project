using AttendanceSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<Class> GetByCodeAsync(string code);
        Task<IEnumerable<Class>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Class>> GetActiveClassesAsync();
    }
}
