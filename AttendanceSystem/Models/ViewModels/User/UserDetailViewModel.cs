using AttendanceSystem.Models;
using System;
using System.Collections.Generic;

namespace AttendanceSystem.ViewModels.User
{
    public class UserDetailViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StudentId { get; set; }
        public UserRole Role { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public IEnumerable<Attendance.AttendanceRecordViewModel> RecentAttendances { get; set; }
        public IEnumerable<Class.ClassSummaryViewModel> AssignedClasses { get; set; }
        public IEnumerable<Event.EventSummaryViewModel> RegisteredEvents { get; set; }
    }
}
