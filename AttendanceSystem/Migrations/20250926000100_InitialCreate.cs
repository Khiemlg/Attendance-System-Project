using System;
using System.Data.Entity.Migrations;

namespace AttendanceSystem.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 20),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .Index(t => t.Code, unique: true, name: "IX_Department_Code");

            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(maxLength: 20),
                        StudentId = c.String(maxLength: 20),
                        Role = c.Int(nullable: false),
                        DepartmentId = c.Int(),
                        FaceImagePath = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.Username, unique: true, name: "IX_User_Username")
                .Index(t => t.Email, unique: true, name: "IX_User_Email")
                .Index(t => t.StudentId, unique: true, name: "IX_User_StudentId")
                .Index(t => t.DepartmentId);

            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 500),
                        TeacherId = c.Int(nullable: false),
                        DepartmentId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.Users", t => t.TeacherId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.Code, unique: true, name: "IX_Class_Code")
                .Index(t => t.TeacherId)
                .Index(t => t.DepartmentId);

            CreateTable(
                "dbo.ClassStudents",
                c => new
                    {
                        ClassStudentId = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        JoinDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.ClassStudentId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .ForeignKey("dbo.Users", t => t.StudentId)
                .Index(t => new { t.ClassId, t.StudentId }, unique: true, name: "IX_ClassStudent_Unique")
                .Index(t => t.StudentId);

            CreateTable(
                "dbo.ClassSessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 200),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        QRCodeData = c.String(maxLength: 500),
                        QRCodeExpiry = c.DateTime(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.Classes", t => t.ClassId)
                .Index(t => t.ClassId);

            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 1000),
                        OrganizerId = c.Int(nullable: false),
                        DepartmentId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 200),
                        MaxParticipants = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RequiresCertificate = c.Boolean(nullable: false, defaultValue: false),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        QRCodeData = c.String(maxLength: 500),
                        QRCodeExpiry = c.DateTime(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Users", t => t.OrganizerId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.Code, unique: true, name: "IX_Event_Code")
                .Index(t => t.OrganizerId)
                .Index(t => t.DepartmentId);

            CreateTable(
                "dbo.EventParticipants",
                c => new
                    {
                        ParticipantId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        IsApproved = c.Boolean(nullable: false, defaultValue: false),
                    })
                .PrimaryKey(t => t.ParticipantId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.EventId, t.UserId }, unique: true, name: "IX_EventParticipant_Unique")
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClassSessionId = c.Int(),
                        EventId = c.Int(),
                        Status = c.Int(nullable: false),
                        CheckInTime = c.DateTime(nullable: false),
                        CheckOutTime = c.DateTime(),
                        AttendanceMethod = c.String(maxLength: 200),
                        Notes = c.String(maxLength: 500),
                        RecordedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.AttendanceId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.ClassSessions", t => t.ClassSessionId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => new { t.ClassSessionId, t.UserId }, unique: true, name: "IX_Attendance_ClassSession_User")
                .Index(t => new { t.EventId, t.UserId }, unique: true, name: "IX_Attendance_Event_User")
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        CertificateId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        CertificateNumber = c.String(nullable: false, maxLength: 100),
                        IssueDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        FilePath = c.String(maxLength: 500),
                        IsSent = c.Boolean(nullable: false, defaultValue: false),
                        SentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CertificateId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.CertificateNumber, unique: true, name: "IX_Certificate_Number")
                .Index(t => t.UserId)
                .Index(t => t.EventId);

            CreateTable(
                "dbo.EventFeedbacks",
                c => new
                    {
                        FeedbackId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(maxLength: 1000),
                        SubmittedDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.FeedbackId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.EventId, t.UserId }, unique: true, name: "IX_EventFeedback_Unique")
                .Index(t => t.UserId);

            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        SettingId = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(nullable: false, maxLength: 100),
                        SettingValue = c.String(maxLength: 500),
                        Description = c.String(maxLength: 200),
                        LastUpdated = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.SettingId)
                .Index(t => t.SettingKey, unique: true, name: "IX_SystemSetting_Key");
        }

        public override void Down()
        {
            DropIndex("dbo.SystemSettings", "IX_SystemSetting_Key");
            DropIndex("dbo.EventFeedbacks", new[] { "UserId" });
            DropIndex("dbo.EventFeedbacks", "IX_EventFeedback_Unique");
            DropIndex("dbo.Certificates", new[] { "EventId" });
            DropIndex("dbo.Certificates", new[] { "UserId" });
            DropIndex("dbo.Certificates", "IX_Certificate_Number");
            DropIndex("dbo.Attendances", new[] { "UserId" });
            DropIndex("dbo.Attendances", "IX_Attendance_Event_User");
            DropIndex("dbo.Attendances", "IX_Attendance_ClassSession_User");
            DropIndex("dbo.EventParticipants", new[] { "UserId" });
            DropIndex("dbo.EventParticipants", "IX_EventParticipant_Unique");
            DropIndex("dbo.Events", new[] { "DepartmentId" });
            DropIndex("dbo.Events", new[] { "OrganizerId" });
            DropIndex("dbo.Events", "IX_Event_Code");
            DropIndex("dbo.ClassSessions", new[] { "ClassId" });
            DropIndex("dbo.ClassStudents", new[] { "StudentId" });
            DropIndex("dbo.ClassStudents", "IX_ClassStudent_Unique");
            DropIndex("dbo.Classes", new[] { "DepartmentId" });
            DropIndex("dbo.Classes", new[] { "TeacherId" });
            DropIndex("dbo.Classes", "IX_Class_Code");
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", "IX_User_StudentId");
            DropIndex("dbo.Users", "IX_User_Email");
            DropIndex("dbo.Users", "IX_User_Username");
            DropIndex("dbo.Departments", "IX_Department_Code");

            DropForeignKey("dbo.EventFeedbacks", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventFeedbacks", "EventId", "dbo.Events");
            DropForeignKey("dbo.Certificates", "UserId", "dbo.Users");
            DropForeignKey("dbo.Certificates", "EventId", "dbo.Events");
            DropForeignKey("dbo.Attendances", "EventId", "dbo.Events");
            DropForeignKey("dbo.Attendances", "ClassSessionId", "dbo.ClassSessions");
            DropForeignKey("dbo.Attendances", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventParticipants", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventParticipants", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Events", "OrganizerId", "dbo.Users");
            DropForeignKey("dbo.ClassSessions", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.ClassStudents", "StudentId", "dbo.Users");
            DropForeignKey("dbo.ClassStudents", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Classes", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Classes", "TeacherId", "dbo.Users");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");

            DropTable("dbo.SystemSettings");
            DropTable("dbo.EventFeedbacks");
            DropTable("dbo.Certificates");
            DropTable("dbo.Attendances");
            DropTable("dbo.EventParticipants");
            DropTable("dbo.Events");
            DropTable("dbo.ClassSessions");
            DropTable("dbo.ClassStudents");
            DropTable("dbo.Classes");
            DropTable("dbo.Users");
            DropTable("dbo.Departments");
        }
    }
}
