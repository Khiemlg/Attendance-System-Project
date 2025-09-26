using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.ViewModels.Event;
using AttendanceSystem.ViewModels.Shared;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventRepository _eventRepository;
        private readonly IExcelService _excelService;
        private readonly ICertificateService _certificateService;

        public EventService(ApplicationDbContext context, IEventRepository eventRepository, IExcelService excelService, ICertificateService certificateService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _certificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
        }

        public async Task<PagedResult<EventSummaryViewModel>> GetEventsAsync(int page, int pageSize, string keyword, int? departmentId, string status)
        {
            var query = _context.Events.Include(e => e.Department).Include(e => e.EventParticipants).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(e => e.Name.Contains(keyword) || e.Code.Contains(keyword));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId);
            }

            if (Enum.TryParse<EventStatus>(status, out var parsedStatus))
            {
                query = query.Where(e => e.Status == parsedStatus);
            }

            var totalItems = await query.CountAsync();
            var events = await query
                .OrderByDescending(e => e.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EventSummaryViewModel
                {
                    EventId = e.EventId,
                    Name = e.Name,
                    Code = e.Code,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Status = e.Status.ToString(),
                    TotalParticipants = e.EventParticipants.Count,
                    DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                    RequiresCertificate = e.RequiresCertificate
                })
                .ToListAsync();

            return new PagedResult<EventSummaryViewModel>
            {
                Items = events,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<EventDetailViewModel> GetEventDetailAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Department)
                .Include(e => e.EventParticipants.Select(ep => ep.User))
                .Include(e => e.Attendances)
                .Include(e => e.EventFeedbacks)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventEntity == null)
            {
                return null;
            }

            var summary = new EventSummaryViewModel
            {
                EventId = eventEntity.EventId,
                Name = eventEntity.Name,
                Code = eventEntity.Code,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Status = eventEntity.Status.ToString(),
                TotalParticipants = eventEntity.EventParticipants.Count,
                DepartmentName = eventEntity.Department?.Name,
                RequiresCertificate = eventEntity.RequiresCertificate
            };

            var participants = eventEntity.EventParticipants.Select(p => new EventParticipantViewModel
            {
                ParticipantId = p.ParticipantId,
                UserId = p.UserId,
                ParticipantName = p.User.FullName,
                Email = p.User.Email,
                IsApproved = p.IsApproved
            });

            var feedbacks = eventEntity.EventFeedbacks.Select(f => new EventFeedbackViewModel
            {
                FeedbackId = f.FeedbackId,
                Rating = f.Rating,
                Comments = f.Comments,
                ParticipantName = f.User.FullName,
                SubmittedDate = f.SubmittedDate
            });

            var sessions = await _context.ClassSessions
                .Where(s => s.ClassId == null && s.SessionId == eventId)
                .Select(s => new EventSessionViewModel
                {
                    SessionId = s.SessionId,
                    Title = s.Title,
                    Description = s.Description,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Location = s.Location,
                    QRCodeUrl = s.QRCodeData
                })
                .ToListAsync();

            return new EventDetailViewModel
            {
                EventInfo = summary,
                Participants = participants,
                Sessions = sessions,
                Feedbacks = feedbacks
            };
        }

        public async Task<int> CreateEventAsync(EventFormViewModel model)
        {
            var eventEntity = new Event
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                OrganizerId = model.OrganizerId,
                DepartmentId = model.DepartmentId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Location = model.Location,
                MaxParticipants = model.MaxParticipants,
                RequiresCertificate = model.RequiresCertificate,
                Status = EventStatus.Planning,
                CreatedDate = DateTime.Now
            };

            await _eventRepository.AddAsync(eventEntity);

            if (model.ParticipantExcelFile != null)
            {
                var tempPath = SaveTempFile(model.ParticipantExcelFile);
                await ImportParticipantsAsync(eventEntity.EventId, tempPath);
                File.Delete(tempPath);
            }

            return eventEntity.EventId;
        }

        public async Task UpdateEventAsync(int eventId, EventFormViewModel model)
        {
            var eventEntity = await _eventRepository.GetAsync(eventId);
            if (eventEntity == null)
            {
                return;
            }

            eventEntity.Name = model.Name;
            eventEntity.Code = model.Code;
            eventEntity.Description = model.Description;
            eventEntity.OrganizerId = model.OrganizerId;
            eventEntity.DepartmentId = model.DepartmentId;
            eventEntity.StartDate = model.StartDate;
            eventEntity.EndDate = model.EndDate;
            eventEntity.Location = model.Location;
            eventEntity.MaxParticipants = model.MaxParticipants;
            eventEntity.RequiresCertificate = model.RequiresCertificate;

            await _eventRepository.UpdateAsync(eventEntity);

            if (model.ParticipantExcelFile != null)
            {
                var tempPath = SaveTempFile(model.ParticipantExcelFile);
                await ImportParticipantsAsync(eventId, tempPath);
                File.Delete(tempPath);
            }
        }

        public async Task ImportParticipantsAsync(int eventId, string filePath)
        {
            var participants = await _excelService.ImportAsync<EventParticipantImportModel>(filePath);
            foreach (var participant in participants)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == participant.Email || u.StudentId == participant.Code);
                if (user == null)
                {
                    continue;
                }

                var exists = await _context.EventParticipants.AnyAsync(ep => ep.EventId == eventId && ep.UserId == user.UserId);
                if (exists)
                {
                    continue;
                }

                _context.EventParticipants.Add(new EventParticipant
                {
                    EventId = eventId,
                    UserId = user.UserId,
                    RegistrationDate = DateTime.Now,
                    IsApproved = true
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventSummaryViewModel>> GetActiveEventsForMobileAsync()
        {
            var now = DateTime.Now;
            return await _context.Events
                .Where(e => e.StartDate <= now && e.EndDate >= now)
                .Select(e => new EventSummaryViewModel
                {
                    EventId = e.EventId,
                    Name = e.Name,
                    Code = e.Code,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Status = e.Status.ToString(),
                    TotalParticipants = e.EventParticipants.Count,
                    DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                    RequiresCertificate = e.RequiresCertificate
                })
                .ToListAsync();
        }

        public async Task AssignCertificateAsync(int eventId)
        {
            var eventEntity = await _context.Events.Include(e => e.EventParticipants).FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventEntity == null || !eventEntity.RequiresCertificate)
            {
                return;
            }

            foreach (var participant in eventEntity.EventParticipants)
            {
                await _certificateService.GenerateCertificateAsync(eventId, participant.UserId);
            }

            eventEntity.Status = EventStatus.Completed;
            await _context.SaveChangesAsync();
        }

        private async Task GenerateQrCodeAsync(int eventId)
        {
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                return;
            }

            var qrData = $"EVENT:{eventId}:{Guid.NewGuid()}";
            using (var generator = new QRCodeGenerator())
            {
                var data = generator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(data);
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    var folder = Path.Combine("Uploads", "QR");
                    Directory.CreateDirectory(folder);
                    var filePath = Path.Combine(folder, $"event-{eventId}.png");
                    bitmap.Save(filePath);
                    eventEntity.QRCodeData = filePath;
                    eventEntity.QRCodeExpiry = DateTime.Now.AddMinutes(30);
                }
            }

            await _context.SaveChangesAsync();
        }

        private string SaveTempFile(System.Web.HttpPostedFileBase file)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "AttendanceSystem");
            Directory.CreateDirectory(tempFolder);
            var filePath = Path.Combine(tempFolder, Guid.NewGuid() + Path.GetExtension(file.FileName));
            file.SaveAs(filePath);
            return filePath;
        }

        private class EventParticipantImportModel
        {
            public string Email { get; set; }
            public string Code { get; set; }
        }
    }
}
