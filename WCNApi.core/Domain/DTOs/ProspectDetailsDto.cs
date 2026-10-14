namespace WCNApi.core.Domain.DTOs
{
    public class ProspectDetailsDto
    {
        public int PId { get; set; }
        public string? NameofBusiness { get; set; }
        public string? DBA { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? OwnerContact { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Industry { get; set; }
        public string? ProductType { get; set; }
        public string? Agency { get; set; }
        public string? Status { get; set; }
        public int? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? SalesRepId { get; set; }
    }
}
