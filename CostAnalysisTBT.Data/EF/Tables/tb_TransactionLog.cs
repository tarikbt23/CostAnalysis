using System;
using System.Collections.Generic;

namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_TransactionLog
    {
        public tb_TransactionLog()
        {
            TransactionLogRow = new HashSet<tb_TransactionLogRow>();
        }

        public int TransactionLogRef { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<tb_TransactionLogRow> TransactionLogRow { get; set; }
    }
}
