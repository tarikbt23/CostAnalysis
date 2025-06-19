using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_IncomeStatementRow
    {
        public int IncomeStatementRowRef { get; set; }
        public int IncomeStatementRef { get; set; }
        public int IncomeDate { get; set; }
        public string IncomeSource { get; set; }
        public string Client  { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public virtual tb_IncomeStatement IncomeStatement { get; set; }
    }
}
