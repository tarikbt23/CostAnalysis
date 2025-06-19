namespace CostAnalysisTBT.Business.Models
{
    public abstract class CostAnalysisRowBase
    {
        public int RowIndex { get; set; }
        public string Status { get; set; } = "1";
        public string UploadDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string User { get; set; } = "tarik";
        public bool IsValid { get; set; } = true;
        public Exception Exception { get; set; }
    }
}
