namespace Whispeed_BiancaSaguban.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public int ReporterID { get; set; }
        public string TargetType { get; set; }
        public int TargetID { get; set; }
        public string Reason { get; set; }
        public DateTime DateReported { get; set; }

        public User Reporter { get; set; }
    }
}
