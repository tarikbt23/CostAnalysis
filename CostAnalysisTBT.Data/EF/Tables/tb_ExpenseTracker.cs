using System;
using System.Collections.Generic;

namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_ExpenseTracker
    {
        public tb_ExpenseTracker()
        {
            ExpenseTrackerRow = new HashSet<tb_ExpenseTrackerRow>();
        }

        public int ExpenseTrackerRef { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<tb_ExpenseTrackerRow> ExpenseTrackerRow { get; set; }
    }
}
