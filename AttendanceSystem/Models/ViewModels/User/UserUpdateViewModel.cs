using AttendanceSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.ViewModels.User
{
    public class UserUpdateViewModel
    {
        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mã sinh viên")]
        public string StudentId { get; set; }

        [Display(Name = "Vai trò")]
        public UserRole Role { get; set; }

        [Display(Name = "Khoa/Bộ phận")]
        public int? DepartmentId { get; set; }
    }
}
