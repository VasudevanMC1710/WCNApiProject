namespace WCNApi.core.Domain.DTOs
{
    public class UserDashboardDto
    {
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TodayLoginCount { get; set; }
    }
}
