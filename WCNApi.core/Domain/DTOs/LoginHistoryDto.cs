namespace WCNApi.core.Domain.DTOs
{
    public class LoginHistoryDto
    {
        public int Lid { get; set; }
        public DateTime? LoginDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
