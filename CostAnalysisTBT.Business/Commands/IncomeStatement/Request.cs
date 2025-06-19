using MediatR;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Response;

namespace CostAnalysisTBT.Business.Commands.IncomeStatement
{
    public class Request : IRequest<ExcelUploadResponse>
    {
        public string CreatedUser { get; set; }
        public bool IsOverwrite { get; set; }

        public List<CostAnalysisIncomeStatementRow> Rows { get; set; }
    }
}
