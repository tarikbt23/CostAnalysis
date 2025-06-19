using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Services.ExcelDataOverWrite
{
    public interface IExcelDataOverwriteService
    {
        Task OverwriteAsync(Type rowType, Type headerType, CancellationToken cancellationToken);
    }
}
