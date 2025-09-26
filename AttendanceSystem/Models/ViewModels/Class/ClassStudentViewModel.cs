namespace AttendanceSystem.ViewModels.Class
{
    public class ClassStudentViewModel
    {
        public int ClassStudentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentCode { get; set; }
        public bool IsActive { get; set; }
    }
}
