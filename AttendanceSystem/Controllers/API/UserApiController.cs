using AttendanceSystem.Services;
using AttendanceSystem.ViewModels.Account;
using AttendanceSystem.ViewModels.Shared;
using AttendanceSystem.ViewModels.User;
using System.Threading.Tasks;
using System.Web.Http;

namespace AttendanceSystem.Controllers.API
{
    [RoutePrefix("api/users")]
    public class UserApiController : ApiController
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = await _userService.CreateUserAsync(model);
            return Ok(ApiResponse<int>.Ok(userId, "Đăng ký thành công"));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(ApiResponse<UserDetailViewModel>.Ok(user));
        }
    }
}
