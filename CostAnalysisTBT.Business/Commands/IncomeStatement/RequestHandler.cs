using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Response;
using CostAnalysisTBT.Business.Services;
using CostAnalysisTBT.Business.Services.ExcelDataOverWrite;
using CostAnalysisTBT.Data.EF;
using CostAnalysisTBT.Data.EF.Tables;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.IncomeStatement
{
    public class RequestHandler
        : BaseExcelUploadHandler<
            Request,
            CostAnalysisIncomeStatementRow,
            tb_IncomeStatementRow,
            tb_IncomeStatement>
    {
        public RequestHandler(
            CostAnalysisDbContext context,
            IMapper mapper,
            InvalidRowExcelExporter exporter,
            IExcelDataOverwriteService overwriteService
            )
            : base(context, mapper, exporter,overwriteService)
        {
        }

        protected override IValidator<List<CostAnalysisIncomeStatementRow>> Validator
            => new Validator();

        protected override List<CostAnalysisIncomeStatementRow> GetRows(Request request)
        {
            return request.Rows;
        }
    }
}
