using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Account;
using AttendanceSystem.ViewModels.User;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Index(int page = 1, int pageSize = 20, string keyword = null, int? departmentId = null, string role = null)
        {
            var users = await _userService.GetUsersAsync(page, pageSize, keyword, departmentId, role);
            return View(users);
        }

        public async Task<ActionResult> Details(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.CreateUserAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UserUpdateViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StudentId = user.StudentId,
                Role = user.Role,
                DepartmentId = null
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.UpdateUserAsync(id, model);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<ActionResult> ToggleActive(int id, bool isActive)
        {
            await _userService.ToggleActiveAsync(id, isActive);
            return RedirectToAction("Details", new { id });
        }
    }
}
