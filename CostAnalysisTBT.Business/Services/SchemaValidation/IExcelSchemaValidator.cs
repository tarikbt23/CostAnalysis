using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Services.SchemaValidation
{
    public interface IExcelSchemaValidator
    {
        void ValidateSchema(string fileType, ExcelWorksheet sheet);
    }
}
