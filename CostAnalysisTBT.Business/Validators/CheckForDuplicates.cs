using System.Reflection;
using CostAnalysisTBT.Business.Models;

namespace CostAnalysisTBT.Business.Validators
{
    public class CheckForDuplicates
    {
        public void Execute<T>(List<T> rows, string[] uniqueColumns) where T : CostAnalysisRowBase
        {
            var duplicateCheck = new Dictionary<string, List<int>>();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var key = string.Join("|", uniqueColumns
                    .Select(col => GetPropertyValue(row, col)?.ToString()?.Trim() ?? string.Empty));

                if (duplicateCheck.ContainsKey(key))
                    duplicateCheck[key].Add(i);
                else
                    duplicateCheck[key] = new List<int> { i };
            }

            foreach (var entry in duplicateCheck)
            {
                if (entry.Value.Count > 1)
                {
                    string errorMessage = $"Duplicate rows found at: {string.Join(", ", entry.Value.Select(v => v + 2))}";
                    foreach (var rowIndex in entry.Value)
                    {
                        var duplicateRow = rows[rowIndex];
                        duplicateRow.Exception = new Exception(errorMessage);
                        duplicateRow.IsValid = false;
                    }
                }
            }
        }

        private object? GetPropertyValue(object obj, string propName)
        {
            return obj.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?.GetValue(obj);
        }
    }
}
