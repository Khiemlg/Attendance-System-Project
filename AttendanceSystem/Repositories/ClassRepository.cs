using AttendanceSystem.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Class> GetByCodeAsync(string code)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IEnumerable<Class>> GetByDepartmentAsync(int departmentId)
        {
            return await DbSet.Where(c => c.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetActiveClassesAsync()
        {
            return await DbSet.Where(c => c.IsActive).ToListAsync();
        }
    }
}
