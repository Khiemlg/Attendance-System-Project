using AttendanceSystem.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await DbSet.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            return await DbSet.Where(u => u.Username.Contains(keyword) || u.FullName.Contains(keyword) || u.Email.Contains(keyword)).ToListAsync();
        }
    }
}
