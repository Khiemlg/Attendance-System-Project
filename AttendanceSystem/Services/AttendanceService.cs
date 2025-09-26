using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IRealtimeService _realtimeService;

        public AttendanceService(ApplicationDbContext context, IAttendanceRepository attendanceRepository, IRealtimeService realtimeService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _realtimeService = realtimeService ?? throw new ArgumentNullException(nameof(realtimeService));
        }

        public async Task RecordAttendanceAsync(AttendanceRecordRequest request)
        {
            var attendance = new Attendance
            {
                UserId = request.UserId,
                ClassSessionId = request.ClassSessionId,
                EventId = request.EventId,
                AttendanceMethod = request.Method,
                Status = AttendanceStatus.Present,
                CheckInTime = DateTime.Now,
                RecordedDate = DateTime.Now
            };

            await _attendanceRepository.AddAsync(attendance);

            var realtime = await GetRealtimeAttendanceAsync(request.ClassSessionId ?? request.EventId ?? 0,
                request.ClassSessionId.HasValue ? "ClassSession" : "Event");

            await _realtimeService.BroadcastAttendanceAsync(realtime);
        }

        public async Task<IEnumerable<AttendanceRecordViewModel>> GetAttendanceBySessionAsync(int sessionId)
        {
            return await _context.Attendances
                .Where(a => a.ClassSessionId == sessionId)
                .OrderByDescending(a => a.CheckInTime)
                .Select(a => new AttendanceRecordViewModel
                {
                    AttendanceId = a.AttendanceId,
                    UserId = a.UserId,
                    UserName = a.User.FullName,
                    Status = a.Status.ToString(),
                    Method = a.AttendanceMethod,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    Notes = a.Notes
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceRecordViewModel>> GetAttendanceByEventAsync(int eventId)
        {
            return await _context.Attendances
                .Where(a => a.EventId == eventId)
                .OrderByDescending(a => a.CheckInTime)
                .Select(a => new AttendanceRecordViewModel
                {
                    AttendanceId = a.AttendanceId,
                    UserId = a.UserId,
                    UserName = a.User.FullName,
                    Status = a.Status.ToString(),
                    Method = a.AttendanceMethod,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    Notes = a.Notes
                })
                .ToListAsync();
        }

        public async Task<AttendanceRealtimeViewModel> GetRealtimeAttendanceAsync(int entityId, string entityType)
        {
            var query = _context.Attendances.AsQueryable();

            if (string.Equals(entityType, "ClassSession", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.ClassSessionId == entityId);
            }
            else
            {
                query = query.Where(a => a.EventId == entityId);
            }

            var records = await query
                .OrderByDescending(a => a.CheckInTime)
                .Select(a => new AttendanceRecordViewModel
                {
                    AttendanceId = a.AttendanceId,
                    UserId = a.UserId,
                    UserName = a.User.FullName,
                    Status = a.Status.ToString(),
                    Method = a.AttendanceMethod,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    Notes = a.Notes
                })
                .ToListAsync();

            return new AttendanceRealtimeViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                Title = entityType == "ClassSession"
                    ? (await _context.ClassSessions.Where(c => c.SessionId == entityId).Select(c => c.Title).FirstOrDefaultAsync())
                    : (await _context.Events.Where(e => e.EventId == entityId).Select(e => e.Name).FirstOrDefaultAsync()),
                Records = records
            };
        }
    }
}
