using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.ViewModels.Account;
using AttendanceSystem.ViewModels.Shared;
using AttendanceSystem.ViewModels.User;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserService(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<PagedResult<UserListItemViewModel>> GetUsersAsync(int page, int pageSize, string keyword, int? departmentId, string role)
        {
            var query = _context.Users.Include(u => u.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(u => u.Username.Contains(keyword) || u.FullName.Contains(keyword) || u.Email.Contains(keyword));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(u => u.DepartmentId == departmentId);
            }

            if (Enum.TryParse<UserRole>(role, out var parsedRole))
            {
                query = query.Where(u => u.Role == parsedRole);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListItemViewModel
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    DepartmentName = u.Department != null ? u.Department.Name : string.Empty,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreatedDate
                })
                .ToListAsync();

            return new PagedResult<UserListItemViewModel>
            {
                Items = items,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<UserDetailViewModel> GetUserAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return null;
            }

            var recentAttendances = await _context.Attendances
                .Where(a => a.UserId == id)
                .OrderByDescending(a => a.CheckInTime)
                .Take(10)
                .Select(a => new ViewModels.Attendance.AttendanceRecordViewModel
                {
                    AttendanceId = a.AttendanceId,
                    UserId = a.UserId,
                    UserName = user.FullName,
                    Status = a.Status.ToString(),
                    Method = a.AttendanceMethod,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    Notes = a.Notes
                })
                .ToListAsync();

            return new UserDetailViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StudentId = user.StudentId,
                Role = user.Role,
                DepartmentName = user.Department?.Name,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                RecentAttendances = recentAttendances
            };
        }

        public async Task<int> CreateUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                StudentId = model.StudentId,
                Role = model.Role,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            return user.UserId;
        }

        public async Task UpdateUserAsync(int id, UserUpdateViewModel model)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return;
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.StudentId = model.StudentId;
            user.Role = model.Role;
            user.DepartmentId = model.DepartmentId;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ToggleActiveAsync(int id, bool isActive)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return;
            }

            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);
        }
    }
}
