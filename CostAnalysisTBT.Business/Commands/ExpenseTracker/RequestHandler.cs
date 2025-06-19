using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Response;
using CostAnalysisTBT.Business.Services;
using CostAnalysisTBT.Business.Services.ExcelDataOverWrite;
using CostAnalysisTBT.Business.Validators;
using CostAnalysisTBT.Data.EF;
using CostAnalysisTBT.Data.EF.Tables;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.ExpenseTracker
{
    public class RequestHandler
        : BaseExcelUploadHandler<
            Request,
            CostAnalysisExpenseTrackerRow,
            tb_ExpenseTrackerRow,
            tb_ExpenseTracker>
    {
        public RequestHandler(
            CostAnalysisDbContext context,
            IMapper mapper,
            InvalidRowExcelExporter exporter,
            IExcelDataOverwriteService overwriteService
            )
            : base(context, mapper, exporter, overwriteService)
        {
        }

        protected override IValidator<List<CostAnalysisExpenseTrackerRow>> Validator
            => new Validator();

        protected override List<CostAnalysisExpenseTrackerRow> GetRows(Request request)
        {
            return request.Rows;
        }
    }
}
