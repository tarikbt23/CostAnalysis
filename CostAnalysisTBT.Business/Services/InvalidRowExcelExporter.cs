using System.Reflection;
using OfficeOpenXml;
using CostAnalysisTBT.Business.Models;

namespace CostAnalysisTBT.Business.Services
{
    public class InvalidRowExcelExporter
    {
        public byte[] Export<T>(List<T> invalidRows) where T : CostAnalysisRowBase
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Hatalı Satırlar");

            if (invalidRows == null || invalidRows.Count == 0)
                return [];

            var runtimeType = invalidRows.First().GetType();

            var allProps = runtimeType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var otherProps = allProps.Where(p => p.Name != nameof(CostAnalysisRowBase.RowIndex)).ToList();

            worksheet.Cells[1, 1].Value = "RowIndex";
            for (int i = 0; i < otherProps.Count; i++)
            {
                worksheet.Cells[1, i + 2].Value = otherProps[i].Name;
            }

            for (int rowIdx = 0; rowIdx < invalidRows.Count; rowIdx++)
            {
                var row = invalidRows[rowIdx];
                worksheet.Cells[rowIdx + 2, 1].Value = row.RowIndex;

                for (int colIdx = 0; colIdx < otherProps.Count; colIdx++)
                {
                    var value = otherProps[colIdx].GetValue(row);
                    worksheet.Cells[rowIdx + 2, colIdx + 2].Value = value?.ToString();
                }

            }

            return package.GetAsByteArray();
        }

    }
}
