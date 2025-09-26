using AttendanceSystem.Repositories;
using AttendanceSystem.Services;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace AttendanceSystem
{
    public static class DependencyConfig
    {
        public static void Register()
        {
            var container = new UnityContainer();

            // Repositories
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IClassRepository, ClassRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEventRepository, EventRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAttendanceRepository, AttendanceRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportRepository, ReportRepository>(new HierarchicalLifetimeManager());

            // Services
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IClassService, ClassService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEventService, EventService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAttendanceService, AttendanceService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRealtimeService, RealtimeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IExcelService, ExcelService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICertificateService, CertificateService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailService, EmailService>(new HierarchicalLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
