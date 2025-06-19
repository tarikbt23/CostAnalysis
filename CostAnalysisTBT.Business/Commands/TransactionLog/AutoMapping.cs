using AutoMapper;
using CostAnalysisTBT.Business.Models;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Business.Commands.TransactionLog
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CostAnalysisTransactionLogRow, tb_TransactionLogRow>()
                .ForMember(dest => dest.TransactionLogRef, opt => opt.Ignore());
        }
    }

}
