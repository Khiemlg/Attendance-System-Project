using AttendanceSystem.Models;
using AttendanceSystem.ViewModels.Reports;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttendanceSummaryViewModel> GetClassAttendanceSummaryAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.ClassStudents)
                .Include(c => c.ClassSessions.Select(s => s.Attendances))
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classEntity == null)
            {
                return null;
            }

            var totalSessions = classEntity.ClassSessions.Count;
            var totalStudents = classEntity.ClassStudents.Count;
            var totalAttendances = classEntity.ClassSessions.Sum(s => s.Attendances.Count);

            return new AttendanceSummaryViewModel
            {
                ClassId = classEntity.ClassId,
                ClassName = classEntity.Name,
                TotalSessions = totalSessions,
                TotalStudents = totalStudents,
                TotalAttendances = totalAttendances,
                PresentCount = classEntity.ClassSessions.Sum(s => s.Attendances.Count(a => a.Status == AttendanceStatus.Present)),
                AbsentCount = classEntity.ClassSessions.Sum(s => s.Attendances.Count(a => a.Status == AttendanceStatus.Absent)),
                LateCount = classEntity.ClassSessions.Sum(s => s.Attendances.Count(a => a.Status == AttendanceStatus.Late)),
                ExcusedCount = classEntity.ClassSessions.Sum(s => s.Attendances.Count(a => a.Status == AttendanceStatus.Excused))
            };
        }

        public async Task<EventSummaryViewModel> GetEventSummaryAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Attendances)
                .Include(e => e.EventParticipants)
                .Include(e => e.EventFeedbacks)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventEntity == null)
            {
                return null;
            }

            return new EventSummaryViewModel
            {
                EventId = eventEntity.EventId,
                EventName = eventEntity.Name,
                EventCode = eventEntity.Code,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                TotalParticipants = eventEntity.EventParticipants.Count,
                TotalCheckIns = eventEntity.Attendances.Count(a => a.Status == AttendanceStatus.Present),
                FeedbackCount = eventEntity.EventFeedbacks?.Count ?? 0,
                AverageRating = eventEntity.EventFeedbacks?.Any() == true ? eventEntity.EventFeedbacks.Average(f => f.Rating) : 0
            };
        }

        public async Task<SystemDashboardViewModel> GetSystemDashboardAsync()
        {
            var dashboard = new SystemDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalClasses = await _context.Classes.CountAsync(),
                TotalEvents = await _context.Events.CountAsync(),
                ActiveClasses = await _context.Classes.CountAsync(c => c.IsActive),
                ActiveEvents = await _context.Events.CountAsync(e => e.Status == EventStatus.Active),
                TotalCertificatesIssued = await _context.Certificates.CountAsync(c => c.IsSent)
            };

            return dashboard;
        }
    }
}
