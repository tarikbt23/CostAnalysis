using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_IncomeStatement
    {
        public tb_IncomeStatement()
        {
            IncomeStatementRow = new HashSet<tb_IncomeStatementRow>();
        }

        public int IncomeStatementRef { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<tb_IncomeStatementRow> IncomeStatementRow { get; set; }
    }
}
