using System;
using System.Linq;
using System.Reflection;
using CostAnalysisTBT.Business.Models;

namespace CostAnalysisTBT.Business.Services
{
    public class CheckForEmptyFields
    {
        public void Execute<T>(List<T> rows, string[] requiredColumns) where T : CostAnalysisRowBase
        {
            foreach (var row in rows)
            {
                foreach (var col in requiredColumns)
                {
                    var prop = row.GetType()
                                  .GetProperty(col, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var val = prop?.GetValue(row)?.ToString();

                    if (string.IsNullOrWhiteSpace(val))
                    {
                        row.IsValid = false;
                        row.Exception = new Exception($"{col} kolonu boş olamaz.");
                        break; 
                    }
                }
            }
        }
    }
}
