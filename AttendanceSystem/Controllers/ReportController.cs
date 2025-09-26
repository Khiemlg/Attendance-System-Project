using AttendanceSystem.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "Admin,Teacher,EventOrganizer")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<ActionResult> SystemDashboard()
        {
            var dashboard = await _reportRepository.GetSystemDashboardAsync();
            return View(dashboard);
        }

        public async Task<ActionResult> ClassSummary(int id)
        {
            var summary = await _reportRepository.GetClassAttendanceSummaryAsync(id);
            if (summary == null)
            {
                return HttpNotFound();
            }

            return View(summary);
        }

        public async Task<ActionResult> EventSummary(int id)
        {
            var summary = await _reportRepository.GetEventSummaryAsync(id);
            if (summary == null)
            {
                return HttpNotFound();
            }

            return View(summary);
        }
    }
}
