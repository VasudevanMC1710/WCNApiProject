using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
