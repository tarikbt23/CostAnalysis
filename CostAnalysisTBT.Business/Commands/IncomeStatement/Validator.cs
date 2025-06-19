using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Validators;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.IncomeStatement
{
    public class Validator : AbstractValidator<List<CostAnalysisIncomeStatementRow>>
    {
        public Validator()
        {
            RuleForEach(row => row).ChildRules(row =>
            {
                row.RuleFor(x => x.IncomeDate)
                   .NotEmpty().WithMessage("IncomeDate kolonu boş olamaz.");

                row.RuleFor(x => x.IncomeSource)
                   .NotEmpty().WithMessage("IncomeSource kolonu boş olamaz.");

                row.RuleFor(x => x.Client )
                   .NotEmpty().WithMessage("Client  kolonu boş olamaz.");

                row.RuleFor(x => x.Amount)
                   .NotEmpty().WithMessage("Amount kolonu boş olamaz.");

            });
        }
    }
}
