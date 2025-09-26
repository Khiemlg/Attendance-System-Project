using System.Collections.Generic;

namespace AttendanceSystem.ViewModels.Shared
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
    }
}
