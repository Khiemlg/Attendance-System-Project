using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Models
{
    public enum UserRole
    {
        Admin = 1,
        Teacher = 2,
        Student = 3,
        EventOrganizer = 4
    }

    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        Excused = 4
    }

    public enum EventStatus
    {
        Planning = 1,
        Active = 2,
        Completed = 3,
        Cancelled = 4
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string StudentId { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(500)]
        public string FaceImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<EventParticipant> EventParticipants { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Class> CreatedClasses { get; set; }
        public virtual ICollection<Event> CreatedEvents { get; set; }
    }

    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }

    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int TeacherId { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<ClassSession> ClassSessions { get; set; }
    }

    public class ClassStudent
    {
        [Key]
        public int ClassStudentId { get; set; }

        public int ClassId { get; set; }

        public int StudentId { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }
    }

    public class ClassSession
    {
        [Key]
        public int SessionId { get; set; }

        public int ClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // QR Code for attendance
        [StringLength(500)]
        public string QRCodeData { get; set; }

        public DateTime? QRCodeExpiry { get; set; }

        // Navigation properties
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int OrganizerId { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public int MaxParticipants { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Planning;

        public bool RequiresCertificate { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // QR Code for attendance
        [StringLength(500)]
        public string QRCodeData { get; set; }

        public DateTime? QRCodeExpiry { get; set; }

        // Navigation properties
        [ForeignKey("OrganizerId")]
        public virtual User Organizer { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<EventParticipant> EventParticipants { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<EventFeedback> EventFeedbacks { get; set; }
    }

    public class EventParticipant
    {
        [Key]
        public int ParticipantId { get; set; }

        public int EventId { get; set; }

        public int UserId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        // Navigation properties
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int UserId { get; set; }

        public int? ClassSessionId { get; set; }

        public int? EventId { get; set; }

        public AttendanceStatus Status { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [StringLength(200)]
        public string AttendanceMethod { get; set; } // QR, Face Recognition

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime RecordedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ClassSessionId")]
        public virtual ClassSession ClassSession { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }

    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        public int UserId { get; set; }

        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string CertificateNumber { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string FilePath { get; set; }

        public bool IsSent { get; set; } = false;

        public DateTime? SentDate { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }

    public class EventFeedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public int UserId { get; set; }

        public int EventId { get; set; }

        public int Rating { get; set; } // 1-5

        [StringLength(1000)]
        public string Comments { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }

    public class SystemSettings
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        [StringLength(100)]
        public string SettingKey { get; set; }

        [StringLength(500)]
        public string SettingValue { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}