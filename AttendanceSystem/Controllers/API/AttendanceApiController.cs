using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Attendance;
using AttendanceSystem.ViewModels.Shared;
using System.Threading.Tasks;
using System.Web.Http;

namespace AttendanceSystem.Controllers.API
{
    [RoutePrefix("api/attendance")]
    public class AttendanceApiController : ApiController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceApiController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost]
        [Route("record")]
        public async Task<IHttpActionResult> RecordAttendance(AttendanceRecordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _attendanceService.RecordAttendanceAsync(request);
            return Ok(ApiResponse<string>.Ok("Attendance recorded"));
        }

        [HttpGet]
        [Route("realtime")]
        public async Task<IHttpActionResult> GetRealtime(int entityId, string entityType)
        {
            var realtime = await _attendanceService.GetRealtimeAttendanceAsync(entityId, entityType);
            return Ok(ApiResponse<AttendanceRealtimeViewModel>.Ok(realtime));
        }
    }
}
