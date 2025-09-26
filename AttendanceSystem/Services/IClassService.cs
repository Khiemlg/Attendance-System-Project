using AttendanceSystem.ViewModels.Class;
using AttendanceSystem.ViewModels.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IClassService
    {
        Task<PagedResult<ClassSummaryViewModel>> GetClassesAsync(int page, int pageSize, string keyword, int? departmentId, bool? isActive);
        Task<ClassDetailViewModel> GetClassDetailAsync(int classId);
        Task<int> CreateClassAsync(ClassFormViewModel model);
        Task UpdateClassAsync(int classId, ClassFormViewModel model);
        Task<IEnumerable<ClassSessionViewModel>> GetUpcomingSessionsAsync();
        Task ImportStudentsAsync(int classId, string filePath);
        Task GenerateQrCodeAsync(int sessionId);
    }
}
