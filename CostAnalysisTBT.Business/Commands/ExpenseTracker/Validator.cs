using System;
using System.Collections.Generic;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Business.Services;    
using CostAnalysisTBT.Business.Validators;
using FluentValidation;

namespace CostAnalysisTBT.Business.Commands.ExpenseTracker
{
    public class Validator : AbstractValidator<List<CostAnalysisExpenseTrackerRow>>
    {
        public Validator()
        {
            RuleFor(rows => rows).Custom((rows, context) =>
            {
                var emptyChecker = new CheckForEmptyFields();
                emptyChecker.Execute(rows, new[]
                {
                    nameof(CostAnalysisExpenseTrackerRow.ExpenseDate),
                    nameof(CostAnalysisExpenseTrackerRow.ExpenseCategory),
                    nameof(CostAnalysisExpenseTrackerRow.Description),
                    nameof(CostAnalysisExpenseTrackerRow.Amount)
                });

                var duplicateChecker = new CheckForDuplicates();
                duplicateChecker.Execute(rows, new[]
                {
                    nameof(CostAnalysisExpenseTrackerRow.ExpenseDate),
                    nameof(CostAnalysisExpenseTrackerRow.ExpenseCategory)
                });

                foreach (var row in rows)
                {
                    if (!row.IsValid && row.Exception != null)
                    {
                        context.AddFailure($"Satır {row.RowIndex}: {row.Exception.Message}");
                    }
                }
            });
        }
    }
}
