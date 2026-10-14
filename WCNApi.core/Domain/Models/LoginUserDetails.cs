using System.ComponentModel.DataAnnotations;

namespace WCNApi.core.Domain.Models
{
    public class LoginUserDetails
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
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
    }
}
