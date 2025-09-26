using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.ViewModels.Class;
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
    public class ClassService : IClassService
    {
        private readonly ApplicationDbContext _context;
        private readonly IClassRepository _classRepository;
        private readonly IExcelService _excelService;

        public ClassService(ApplicationDbContext context, IClassRepository classRepository, IExcelService excelService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
        }

        public async Task<PagedResult<ClassSummaryViewModel>> GetClassesAsync(int page, int pageSize, string keyword, int? departmentId, bool? isActive)
        {
            var query = _context.Classes.Include(c => c.Teacher).Include(c => c.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => c.Name.Contains(keyword) || c.Code.Contains(keyword));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(c => c.DepartmentId == departmentId);
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            var totalItems = await query.CountAsync();
            var classes = await query
                .OrderByDescending(c => c.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClassSummaryViewModel
                {
                    ClassId = c.ClassId,
                    Name = c.Name,
                    Code = c.Code,
                    TeacherName = c.Teacher.FullName,
                    DepartmentName = c.Department != null ? c.Department.Name : string.Empty,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    TotalSessions = c.ClassSessions.Count,
                    TotalStudents = c.ClassStudents.Count,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return new PagedResult<ClassSummaryViewModel>
            {
                Items = classes,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<ClassDetailViewModel> GetClassDetailAsync(int classId)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Department)
                .Include(c => c.ClassSessions)
                .Include(c => c.ClassStudents.Select(cs => cs.Student))
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classEntity == null)
            {
                return null;
            }

            var classInfo = new ClassSummaryViewModel
            {
                ClassId = classEntity.ClassId,
                Name = classEntity.Name,
                Code = classEntity.Code,
                TeacherName = classEntity.Teacher.FullName,
                DepartmentName = classEntity.Department?.Name,
                StartDate = classEntity.StartDate,
                EndDate = classEntity.EndDate,
                TotalSessions = classEntity.ClassSessions.Count,
                TotalStudents = classEntity.ClassStudents.Count,
                IsActive = classEntity.IsActive
            };

            var sessions = classEntity.ClassSessions.Select(s => new ClassSessionViewModel
            {
                SessionId = s.SessionId,
                Title = s.Title,
                Description = s.Description,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Location = s.Location,
                QRCodeUrl = s.QRCodeData,
                IsActive = s.IsActive
            });

            var students = classEntity.ClassStudents.Select(s => new ClassStudentViewModel
            {
                ClassStudentId = s.ClassStudentId,
                StudentId = s.StudentId,
                StudentName = s.Student.FullName,
                StudentEmail = s.Student.Email,
                StudentCode = s.Student.StudentId,
                IsActive = s.IsActive
            });

            return new ClassDetailViewModel
            {
                ClassInfo = classInfo,
                Sessions = sessions,
                Students = students
            };
        }

        public async Task<int> CreateClassAsync(ClassFormViewModel model)
        {
            var classEntity = new Class
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                TeacherId = model.TeacherId,
                DepartmentId = model.DepartmentId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            await _classRepository.AddAsync(classEntity);

            if (model.StudentExcelFile != null)
            {
                var tempPath = SaveTempFile(model.StudentExcelFile);
                await ImportStudentsAsync(classEntity.ClassId, tempPath);
                File.Delete(tempPath);
            }

            return classEntity.ClassId;
        }

        public async Task UpdateClassAsync(int classId, ClassFormViewModel model)
        {
            var classEntity = await _classRepository.GetAsync(classId);
            if (classEntity == null)
            {
                return;
            }

            classEntity.Name = model.Name;
            classEntity.Code = model.Code;
            classEntity.Description = model.Description;
            classEntity.TeacherId = model.TeacherId;
            classEntity.DepartmentId = model.DepartmentId;
            classEntity.StartDate = model.StartDate;
            classEntity.EndDate = model.EndDate;

            await _classRepository.UpdateAsync(classEntity);

            if (model.StudentExcelFile != null)
            {
                var tempPath = SaveTempFile(model.StudentExcelFile);
                await ImportStudentsAsync(classId, tempPath);
                File.Delete(tempPath);
            }
        }

        public async Task<IEnumerable<ClassSessionViewModel>> GetUpcomingSessionsAsync()
        {
            var now = DateTime.Now;
            return await _context.ClassSessions
                .Where(s => s.StartTime >= now)
                .OrderBy(s => s.StartTime)
                .Take(10)
                .Select(s => new ClassSessionViewModel
                {
                    SessionId = s.SessionId,
                    Title = s.Title,
                    Description = s.Description,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Location = s.Location,
                    QRCodeUrl = s.QRCodeData,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }

        public async Task ImportStudentsAsync(int classId, string filePath)
        {
            var students = await _excelService.ImportAsync<ClassStudentImportModel>(filePath);
            foreach (var student in students)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.StudentId == student.StudentCode);
                if (user == null)
                {
                    continue;
                }

                var exists = await _context.ClassStudents.AnyAsync(cs => cs.ClassId == classId && cs.StudentId == user.UserId);
                if (exists)
                {
                    continue;
                }

                _context.ClassStudents.Add(new ClassStudent
                {
                    ClassId = classId,
                    StudentId = user.UserId,
                    JoinDate = DateTime.Now,
                    IsActive = true
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task GenerateQrCodeAsync(int sessionId)
        {
            var session = await _context.ClassSessions.FindAsync(sessionId);
            if (session == null)
            {
                return;
            }

            var qrData = $"CLASS:{sessionId}:{Guid.NewGuid()}";
            using (var generator = new QRCodeGenerator())
            {
                var data = generator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(data);
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    var folder = Path.Combine("Uploads", "QR");
                    Directory.CreateDirectory(folder);
                    var filePath = Path.Combine(folder, $"class-session-{sessionId}.png");
                    bitmap.Save(filePath);
                    session.QRCodeData = filePath;
                    session.QRCodeExpiry = DateTime.Now.AddMinutes(30);
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

        private class ClassStudentImportModel
        {
            public string StudentCode { get; set; }
        }
    }
}
