using AttendanceSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<IEnumerable<User>> SearchAsync(string keyword);
    }
}
