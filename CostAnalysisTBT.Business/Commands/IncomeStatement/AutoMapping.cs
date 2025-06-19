using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Business.Commands.IncomeStatement
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CostAnalysisIncomeStatementRow, tb_IncomeStatementRow>()
                .ForMember(dest => dest.IncomeStatementRef, opt => opt.Ignore());
        }
    }

}
