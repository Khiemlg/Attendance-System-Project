using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystemProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string StudentId { get; set; }

        public int? DepartmentId { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [StringLength(255)]
        public string PasswordHash { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Class> TaughtClasses { get; set; }
        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<EventParticipant> EventParticipants { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<EventFeedback> EventFeedbacks { get; set; }

        public User()
        {
            TaughtClasses = new HashSet<Class>();
            ClassStudents = new HashSet<ClassStudent>();
            EventParticipants = new HashSet<EventParticipant>();
            Attendances = new HashSet<Attendance>();
            Certificates = new HashSet<Certificate>();
            EventFeedbacks = new HashSet<EventFeedback>();
        }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}