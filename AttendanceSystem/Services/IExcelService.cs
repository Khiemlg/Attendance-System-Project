using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IExcelService
    {
        Task<IEnumerable<T>> ImportAsync<T>(string filePath) where T : class, new();
        Task<string> ExportAsync<T>(IEnumerable<T> data, string sheetName) where T : class;
    }
}
