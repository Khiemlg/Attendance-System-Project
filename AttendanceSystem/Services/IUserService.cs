using AttendanceSystem.ViewModels.Account;
using AttendanceSystem.ViewModels.Shared;
using AttendanceSystem.ViewModels.User;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserListItemViewModel>> GetUsersAsync(int page, int pageSize, string keyword, int? departmentId, string role);
        Task<UserDetailViewModel> GetUserAsync(int id);
        Task<int> CreateUserAsync(RegisterViewModel model);
        Task UpdateUserAsync(int id, UserUpdateViewModel model);
        Task ToggleActiveAsync(int id, bool isActive);
    }
}
