using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Event;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<ActionResult> Index(int page = 1, int pageSize = 20, string keyword = null, int? departmentId = null, string status = null)
        {
            var events = await _eventService.GetEventsAsync(page, pageSize, keyword, departmentId, status);
            return View(events);
        }

        public async Task<ActionResult> Details(int id)
        {
            var eventDetail = await _eventService.GetEventDetailAsync(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }

            return View(eventDetail);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,EventOrganizer")]
        public ActionResult Create()
        {
            return View(new EventFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,EventOrganizer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventId = await _eventService.CreateEventAsync(model);
            return RedirectToAction("Details", new { id = eventId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,EventOrganizer")]
        public async Task<ActionResult> Edit(int id)
        {
            var eventDetail = await _eventService.GetEventDetailAsync(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }

            var model = new EventFormViewModel
            {
                EventId = eventDetail.EventInfo.EventId,
                Name = eventDetail.EventInfo.Name,
                Code = eventDetail.EventInfo.Code,
                StartDate = eventDetail.EventInfo.StartDate,
                EndDate = eventDetail.EventInfo.EndDate,
                RequiresCertificate = eventDetail.EventInfo.RequiresCertificate
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,EventOrganizer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _eventService.UpdateEventAsync(id, model);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,EventOrganizer")]
        public async Task<ActionResult> AssignCertificate(int id)
        {
            await _eventService.AssignCertificateAsync(id);
            return RedirectToAction("Details", new { id });
        }
    }
}
