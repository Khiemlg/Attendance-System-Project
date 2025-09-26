using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AttendanceSystemProject.Models
{
    public class AttendanceSystemContext : DbContext
    {
        public AttendanceSystemContext() : base("DefaultConnection")
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<EventFeedback> EventFeedbacks { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove pluralizing table name convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.TaughtClasses)
                .WithRequired(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .WillCascadeOnDelete(false);

            // Configure Class entity
            modelBuilder.Entity<Class>()
                .HasOptional(c => c.Department)
                .WithMany(d => d.Classes)
                .HasForeignKey(c => c.DepartmentId)
                .WillCascadeOnDelete(false);

            // Configure ClassStudent entity
            modelBuilder.Entity<ClassStudent>()
                .HasRequired(cs => cs.Class)
                .WithMany(c => c.ClassStudents)
                .HasForeignKey(cs => cs.ClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClassStudent>()
                .HasRequired(cs => cs.Student)
                .WithMany(u => u.ClassStudents)
                .HasForeignKey(cs => cs.StudentId)
                .WillCascadeOnDelete(false);

            // Configure ClassSession entity
            modelBuilder.Entity<ClassSession>()
                .HasRequired(cs => cs.Class)
                .WithMany(c => c.ClassSessions)
                .HasForeignKey(cs => cs.ClassId)
                .WillCascadeOnDelete(false);

            // Configure EventParticipant entity
            modelBuilder.Entity<EventParticipant>()
                .HasRequired(ep => ep.Event)
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(ep => ep.EventId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EventParticipant>()
                .HasRequired(ep => ep.User)
                .WithMany(u => u.EventParticipants)
                .HasForeignKey(ep => ep.UserId)
                .WillCascadeOnDelete(false);

            // Configure Attendance entity
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Attendance>()
                .HasOptional(a => a.ClassSession)
                .WithMany(cs => cs.Attendances)
                .HasForeignKey(a => a.ClassSessionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Attendance>()
                .HasOptional(a => a.Event)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EventId)
                .WillCascadeOnDelete(false);

            // Configure Certificate entity
            modelBuilder.Entity<Certificate>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Certificate>()
                .HasRequired(c => c.Event)
                .WithMany(e => e.Certificates)
                .HasForeignKey(c => c.EventId)
                .WillCascadeOnDelete(false);

            // Configure EventFeedback entity
            modelBuilder.Entity<EventFeedback>()
                .HasRequired(ef => ef.User)
                .WithMany(u => u.EventFeedbacks)
                .HasForeignKey(ef => ef.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EventFeedback>()
                .HasRequired(ef => ef.Event)
                .WithMany(e => e.EventFeedbacks)
                .HasForeignKey(ef => ef.EventId)
                .WillCascadeOnDelete(false);

            // Configure unique constraints
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Code)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.StudentId)
                .IsUnique();

            modelBuilder.Entity<Class>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<EventParticipant>()
                .HasIndex(ep => new { ep.EventId, ep.UserId })
                .IsUnique();

            modelBuilder.Entity<Certificate>()
                .HasIndex(c => c.CertificateNumber)
                .IsUnique();

            modelBuilder.Entity<EventFeedback>()
                .HasIndex(ef => new { ef.EventId, ef.UserId })
                .IsUnique();

            modelBuilder.Entity<SystemSetting>()
                .HasIndex(ss => ss.SettingKey)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}