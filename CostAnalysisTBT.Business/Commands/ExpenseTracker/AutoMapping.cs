using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Business.Commands.ExpenseTracker
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CostAnalysisExpenseTrackerRow, tb_ExpenseTrackerRow>()
                .ForMember(dest => dest.ExpenseTrackerRef, opt => opt.Ignore());
        }
    }

}
