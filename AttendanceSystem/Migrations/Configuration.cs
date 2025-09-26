using System.Data.Entity.Migrations;
using AttendanceSystem.Models;

namespace AttendanceSystem.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AttendanceSystem.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Seed default data (departments, users, etc.) here if desired.
        }
    }
}
