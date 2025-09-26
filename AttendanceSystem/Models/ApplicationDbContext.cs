using System.Data.Entity;

namespace AttendanceSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<EventFeedback> EventFeedbacks { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ClassStudents)
                .WithRequired(cs => cs.Student)
                .HasForeignKey(cs => cs.StudentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.EventParticipants)
                .WithRequired(ep => ep.User)
                .HasForeignKey(ep => ep.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Attendances)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedClasses)
                .WithRequired(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedEvents)
                .WithRequired(e => e.Organizer)
                .HasForeignKey(e => e.OrganizerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.ClassSessions)
                .WithRequired(cs => cs.Class)
                .HasForeignKey(cs => cs.ClassId);

            modelBuilder.Entity<Class>()
                .HasMany(c => c.ClassStudents)
                .WithRequired(cs => cs.Class)
                .HasForeignKey(cs => cs.ClassId);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendances)
                .WithOptional(a => a.Event)
                .HasForeignKey(a => a.EventId);

            modelBuilder.Entity<ClassSession>()
                .HasMany(cs => cs.Attendances)
                .WithOptional(a => a.ClassSession)
                .HasForeignKey(a => a.ClassSessionId);
        }
    }
}
