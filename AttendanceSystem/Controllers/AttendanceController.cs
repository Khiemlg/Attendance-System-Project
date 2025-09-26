using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Attendance;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult> ClassSession(int id)
        {
            var records = await _attendanceService.GetAttendanceBySessionAsync(id);
            return View(records);
        }

        [HttpGet]
        public async Task<ActionResult> Event(int id)
        {
            var records = await _attendanceService.GetAttendanceByEventAsync(id);
            return View(records);
        }

        [HttpPost]
        public async Task<ActionResult> Record(AttendanceRecordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Invalid request");
            }

            await _attendanceService.RecordAttendanceAsync(request);
            return new HttpStatusCodeResult(200);
        }

        [HttpGet]
        public async Task<ActionResult> Realtime(int entityId, string entityType)
        {
            var realtime = await _attendanceService.GetRealtimeAttendanceAsync(entityId, entityType);
            return PartialView("_RealtimeRecords", realtime);
        }
    }
}
