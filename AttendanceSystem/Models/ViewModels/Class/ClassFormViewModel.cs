using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AttendanceSystem.ViewModels.Class
{
    public class ClassFormViewModel
    {
        public int? ClassId { get; set; }

        [Required]
        [Display(Name = "Tên lớp")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mã lớp")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Giảng viên phụ trách")]
        public int TeacherId { get; set; }

        [Display(Name = "Khoa/Bộ phận")]
        public int? DepartmentId { get; set; }

        [Required]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Danh sách sinh viên (Excel)")]
        public HttpPostedFileBase StudentExcelFile { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Teachers { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Departments { get; set; }
    }
}
