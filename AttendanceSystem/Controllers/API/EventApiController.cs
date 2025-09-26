using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Event;
using AttendanceSystem.ViewModels.Shared;
using System.Threading.Tasks;
using System.Web.Http;

namespace AttendanceSystem.Controllers.API
{
    [RoutePrefix("api/events")]
    public class EventApiController : ApiController
    {
        private readonly IEventService _eventService;

        public EventApiController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [Route("active")]
        public async Task<IHttpActionResult> GetActiveEvents()
        {
            var events = await _eventService.GetActiveEventsForMobileAsync();
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<EventSummaryViewModel>>.Ok(events));
        }
    }
}
