
namespace WCNApi.core.Domain.Models
{
    public class LoginHistory
    {
        public int Lid { get; set; }
        public int Userid { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime? LoginDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Active { get; set; } =0;
    }
}
