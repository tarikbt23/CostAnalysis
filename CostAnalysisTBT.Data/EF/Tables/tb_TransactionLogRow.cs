using System;
using System.Collections.Generic;

namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_TransactionLogRow
    {
        public int TransactionLogRowRef { get; set; }
        public int TransactionLogRef { get; set; }
        public int TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public virtual tb_TransactionLog TransactionLog { get; set; }
    }
}
