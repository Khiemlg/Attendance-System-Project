using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class ExcelService : IExcelService
    {
        static ExcelService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<IEnumerable<T>> ImportAsync<T>(string filePath) where T : class, new()
        {
            var list = new List<T>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                await package.LoadAsync(File.OpenRead(filePath));
                var worksheet = package.Workbook.Worksheets[0];
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var item = new T();
                    foreach (var property in properties)
                    {
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var header = worksheet.Cells[1, col].Text;
                            if (string.Equals(header, property.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                var cellValue = worksheet.Cells[row, col].Text;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    var converted = Convert.ChangeType(cellValue, property.PropertyType);
                                    property.SetValue(item, converted);
                                }
                                break;
                            }
                        }
                    }
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<string> ExportAsync<T>(IEnumerable<T> data, string sheetName) where T : class
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"export-{Guid.NewGuid()}.xlsx");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }

                int row = 2;
                foreach (var item in data)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].GetValue(item);
                    }
                    row++;
                }

                await package.SaveAsAsync(new FileInfo(filePath));
            }

            return filePath;
        }
    }
}
