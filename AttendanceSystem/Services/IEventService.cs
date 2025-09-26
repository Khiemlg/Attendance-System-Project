using AttendanceSystem.ViewModels.Event;
using AttendanceSystem.ViewModels.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IEventService
    {
        Task<PagedResult<EventSummaryViewModel>> GetEventsAsync(int page, int pageSize, string keyword, int? departmentId, string status);
        Task<EventDetailViewModel> GetEventDetailAsync(int eventId);
        Task<int> CreateEventAsync(EventFormViewModel model);
        Task UpdateEventAsync(int eventId, EventFormViewModel model);
        Task ImportParticipantsAsync(int eventId, string filePath);
        Task<IEnumerable<EventSummaryViewModel>> GetActiveEventsForMobileAsync();
        Task AssignCertificateAsync(int eventId);
    }
}
