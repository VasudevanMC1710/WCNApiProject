namespace WCNApi.core.Domain.DTOs
{
    public class ProspectSearchRequest
    {
        public string? BusinessName { get; set; }
        public string? State { get; set; }
        public string? Status { get; set; }
        public string? Industry { get; set; }
        public int? SalesRepId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
