namespace CostAnalysisTBT.Business.Models
{
    public class CostAnalysisTransactionLogRow : CostAnalysisRowBase
    {
        public int TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
