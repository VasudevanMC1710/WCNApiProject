namespace WCNApi.core.Domain.DTOs
{
    public class UserWithLoginHistoryDto
    {
        public int Uid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public List<LoginHistoryDto> LoginHistories { get; set; } = new();
    }
}
