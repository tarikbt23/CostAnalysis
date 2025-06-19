using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Services.RowParsing
{
    public class RowParserService : IRowParserService
    {
        public IList ParseRows(string fileType, ExcelWorksheet sheet)
        {
            var rowTypeName = $"CostAnalysisTBT.Business.Models.CostAnalysis{fileType}Row";
            var rowType = Type.GetType(rowTypeName);

            if (rowType == null)
                throw new Exception($"'{fileType}' tipine karşılık gelen satır tipi bulunamadı: {rowTypeName}");

            var listType = typeof(List<>).MakeGenericType(rowType);
            var list = (IList)Activator.CreateInstance(listType)!;

            var props = rowType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            int rowCount = sheet.Dimension.Rows;
            int colCount = sheet.Dimension.Columns;

            for (int r = 2; r <= rowCount; r++)
            {
                var rowInstance = Activator.CreateInstance(rowType);

                for (int c = 1; c <= colCount; c++)
                {
                    if (c > props.Length) break;

                    var prop = props[c - 1];
                    var cellValue = sheet.Cells[r, c].Text?.Trim();

                    var converters = new Dictionary<Type, Func<string, object>>
                    {
                        { typeof(string), s => s },
                        { typeof(int), s => int.TryParse(s, out var i) ? i : 0 },
                        { typeof(decimal), s => decimal.TryParse(s, out var d) ? d : 0 },
                        { typeof(DateTime), s => DateTime.TryParse(s, out var dt) ? dt : DateTime.MinValue },
                        { typeof(bool), s => bool.TryParse(s, out var b) && b }
                    };

                    if (converters.TryGetValue(prop.PropertyType, out var converter))
                    {
                        prop.SetValue(rowInstance, converter(cellValue));
                    }
                }

                var rowIndexProp = rowType.GetProperty("RowIndex");
                rowIndexProp?.SetValue(rowInstance, r);

                list.Add(rowInstance);
            }

            return list;
        }
    }
}
