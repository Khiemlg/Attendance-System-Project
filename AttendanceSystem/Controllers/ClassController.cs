using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Class;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        public async Task<ActionResult> Index(int page = 1, int pageSize = 20, string keyword = null, int? departmentId = null, bool? isActive = null)
        {
            var classes = await _classService.GetClassesAsync(page, pageSize, keyword, departmentId, isActive);
            return View(classes);
        }

        public async Task<ActionResult> Details(int id)
        {
            var classDetail = await _classService.GetClassDetailAsync(id);
            if (classDetail == null)
            {
                return HttpNotFound();
            }
            return View(classDetail);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult Create()
        {
            return View(new ClassFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _classService.CreateClassAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult> Edit(int id)
        {
            var classDetail = await _classService.GetClassDetailAsync(id);
            if (classDetail == null)
            {
                return HttpNotFound();
            }

            var model = new ClassFormViewModel
            {
                ClassId = classDetail.ClassInfo.ClassId,
                Name = classDetail.ClassInfo.Name,
                Code = classDetail.ClassInfo.Code,
                StartDate = classDetail.ClassInfo.StartDate,
                EndDate = classDetail.ClassInfo.EndDate,
                TeacherId = 0
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ClassFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _classService.UpdateClassAsync(id, model);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult> GenerateQr(int classId, int sessionId)
        {
            await _classService.GenerateQrCodeAsync(sessionId);
            return RedirectToAction("Details", new { id = classId });
        }
    }
}
