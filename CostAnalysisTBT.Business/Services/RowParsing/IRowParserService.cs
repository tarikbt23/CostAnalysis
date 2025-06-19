using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Services.RowParsing
{
    public interface IRowParserService
    {
        IList ParseRows(string fileType, ExcelWorksheet sheet);
    }
}
