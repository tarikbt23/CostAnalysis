using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Response;
using CostAnalysisTBT.Business.Services;
using CostAnalysisTBT.Business.Services.ExcelDataOverWrite;
using CostAnalysisTBT.Data.EF;
using CostAnalysisTBT.Data.EF.Tables;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.TransactionLog
{
    public class RequestHandler
        : BaseExcelUploadHandler<
            Request,
            CostAnalysisTransactionLogRow,
            tb_TransactionLogRow,
            tb_TransactionLog> 
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

        protected override IValidator<List<CostAnalysisTransactionLogRow>> Validator
            => new Validator();

        protected override List<CostAnalysisTransactionLogRow> GetRows(Request request)
        {
            return request.Rows;
        }
    }
}
