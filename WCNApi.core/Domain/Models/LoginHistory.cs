
namespace WCNApi.core.Domain.Models
{
    public class LoginHistory
    {
        public int Lid { get; set; }
        public int Userid { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime? LoginDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string Network { get; set; } = string.Empty;
        public string Connectiontype { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string ISP { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Active { get; set; } =0;
    }
}
