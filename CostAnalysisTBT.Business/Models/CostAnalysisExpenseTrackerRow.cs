namespace CostAnalysisTBT.Business.Models
{
    public class CostAnalysisExpenseTrackerRow : CostAnalysisRowBase
    {
        public int ExpenseDate { get; set; }
        public string ExpenseCategory { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
