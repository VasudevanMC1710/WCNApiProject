namespace WCNApi.core.Domain.DTOs
{
    public class UpdateUserRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Active { get; set; }
    }
}
