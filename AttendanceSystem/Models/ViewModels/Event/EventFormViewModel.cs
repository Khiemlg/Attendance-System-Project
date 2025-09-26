using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AttendanceSystem.ViewModels.Event
{
    public class EventFormViewModel
    {
        public int? EventId { get; set; }

        [Required]
        [Display(Name = "Tên sự kiện")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mã sự kiện")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Người tổ chức")]
        public int OrganizerId { get; set; }

        [Display(Name = "Khoa/Bộ phận")]
        public int? DepartmentId { get; set; }

        [Required]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Địa điểm tổ chức")]
        public string Location { get; set; }

        [Display(Name = "Số lượng tham dự tối đa")]
        public int MaxParticipants { get; set; }

        [Display(Name = "Sự kiện cấp chứng nhận")]
        public bool RequiresCertificate { get; set; }

        [Display(Name = "Danh sách tham dự (Excel)")]
        public HttpPostedFileBase ParticipantExcelFile { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Organizers { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Departments { get; set; }
    }
}
