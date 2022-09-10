namespace RookieOnlineAssetManagement.Entities.Dtos.ReportService
{
    public class DetailReportDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int Total { get; set; }
        public int Assigned { get; set; }
        public int Available { get; set; }
        public int NotAvailable { get; set; }
        public int WaitingForRecycling { get; set; }
        public int Recycled { get; set; }
    }
}
