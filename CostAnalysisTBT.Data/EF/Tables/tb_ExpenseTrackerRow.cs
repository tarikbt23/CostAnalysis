namespace CostAnalysisTBT.Data.EF.Tables
{
    public class tb_ExpenseTrackerRow
    {
        public int ExpenseTrackerRowRef { get; set; }
        public int ExpenseTrackerRef { get; set; }

        public int ExpenseDate { get; set; }
        public string ExpenseCategory { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public virtual tb_ExpenseTracker ExpenseTracker { get; set; }
    }
}
