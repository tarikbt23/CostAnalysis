using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Validators;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.TransactionLog
{
    public class Validator : AbstractValidator<List<CostAnalysisTransactionLogRow>>
    {
        public Validator()
        {

        }
    }
}
