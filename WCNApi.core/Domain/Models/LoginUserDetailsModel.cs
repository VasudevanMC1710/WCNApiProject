using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCNApi.core.Domain.Models
{
    public class LoginUserDetailsModel
    {
        public int Uid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Firstname { get; set; } = string.Empty;
        public string Middlename { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string? Deskphone { get; set; }
        public string? Cellphone { get; set; }
        public string? Fax { get; set; }
        public string ReportingManager { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string UserGroup { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public int UserStatus { get; set; }
        public int IsManager { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? RegionalCoordinator { get; set; }
        public int? IsCoordinator { get; set; }
        public int? Active { get; set; }
        public DateTime? Pwddate { get; set; }
        public int? TextNotify { get; set; }
        public int? EmailNotify { get; set; }
        public LoginUserDetailsModel? User { get; set; }
        // Navigation property
        public ICollection<LoginHistoryModel> LoginHistories { get; set; }
            = new List<LoginHistoryModel>();
    }
}
