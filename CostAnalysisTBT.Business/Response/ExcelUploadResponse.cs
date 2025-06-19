using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Response
{
    public class ExcelUploadResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? InvalidRowsExcelFile { get; set; }
        public string? InvalidRowsExcelBase64 { get; set; }
        public string? FileName { get; set; }
    }
}
