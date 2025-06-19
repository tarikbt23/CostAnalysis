using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;
using CostAnalysisTBT.Business.Exceptions;

namespace CostAnalysisTBT.Business.Services.SchemaValidation
{
    public class ExcelSchemaValidator : IExcelSchemaValidator
    {
        public void ValidateSchema(string fileType, ExcelWorksheet sheet)
        {
            var sheetName = sheet.Name?.Trim();
            if (!string.Equals(sheetName, fileType, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException($"Excel kitaplık adı ile yüklenen dosya tipi uyuşmuyor. Beklenen: '{fileType}', Yüklenen: '{sheetName}'");
            }

            var rowTypeName = $"CostAnalysisTBT.Business.Models.CostAnalysis{fileType}Row";
            var rowType = Type.GetType(rowTypeName);
            if (rowType == null)
                throw new BusinessException($"'{fileType}' için Row tipi bulunamadı: {rowTypeName}");

            var ignoredProperties = new List<string> { "RowIndex", "Status", "UploadDate", "User", "IsValid", "Exception" };
            var expectedColumns = rowType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !ignoredProperties.Contains(p.Name))
                .Select(p => p.Name.Trim())
                .ToList();

            int colCount = sheet.Dimension.Columns;
            var actualColumns = new List<string>();
            for (int c = 1; c <= colCount; c++)
            {
                var colName = sheet.Cells[1, c].Text?.Trim();
                if (!string.IsNullOrWhiteSpace(colName))
                    actualColumns.Add(colName);
            }

            var duplicateColumns = actualColumns
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => $"{g.Key} ({g.Count()} kez)")
                .ToList();

            if (duplicateColumns.Any())
            {
                throw new BusinessException($"Excel dosyasında aynı isimli birden fazla kolon var: {string.Join(", ", duplicateColumns)}");
            }

            var missingColumns = expectedColumns
                .Except(actualColumns, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var unexpectedColumns = actualColumns
                .Except(expectedColumns, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (missingColumns.Any() || unexpectedColumns.Any())
            {
                var message = $"Yüklenen dosya '{fileType}' modeline uymuyor.";

                if (missingColumns.Any())
                    message += $" Eksik kolonlar: {string.Join(", ", missingColumns)}.";

                if (unexpectedColumns.Any())
                    message += $" Bu kolon(lar) {fileType} modeline ait değil: {string.Join(", ", unexpectedColumns)}.";

                throw new BusinessException(message);
            }
        }
    }
}
