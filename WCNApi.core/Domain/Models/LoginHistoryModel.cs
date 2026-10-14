using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCNApi.core.Domain.Models
{
    public class LoginHistoryModel
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
        public int Active { get; set; } = 0;
        public LoginUserDetailsModel? User { get; set; }
    }
}
