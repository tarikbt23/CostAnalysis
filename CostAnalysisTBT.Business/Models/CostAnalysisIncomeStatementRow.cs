using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Models
{
    public class CostAnalysisIncomeStatementRow : CostAnalysisRowBase
    {
        public string IncomeDate { get; set; }
        public string IncomeSource { get; set; }
        public string Client  { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
    }
}
