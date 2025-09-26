using System;

namespace AttendanceSystem.ViewModels.Class
{
    public class ClassSummaryViewModel
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TeacherName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSessions { get; set; }
        public int TotalStudents { get; set; }
        public bool IsActive { get; set; }
    }
}
