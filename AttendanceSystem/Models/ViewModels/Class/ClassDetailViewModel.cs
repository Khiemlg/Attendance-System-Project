using System.Collections.Generic;

namespace AttendanceSystem.ViewModels.Class
{
    public class ClassDetailViewModel
    {
        public ClassSummaryViewModel ClassInfo { get; set; }
        public IEnumerable<ClassSessionViewModel> Sessions { get; set; }
        public IEnumerable<ClassStudentViewModel> Students { get; set; }
    }
}
