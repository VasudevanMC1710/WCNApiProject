using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.core.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<LoginUserDetails?> LoginUser(string userName, string password);
        Task<int> LoginHistory(LoginHistory loginHistory);

        Task<LoginUserDetailsModel?> GetUserByIdAsync(int userId);
        Task<bool> AddOrUpdateUserAsync(AddOrUpdateUserRequestDto request);
        Task<UserWithLoginHistoryDto?> GetUserWithLoginHistoryAsync(int userId);
        Task<UserDashboardDto?> GetUserDashboardAsync();
    }
}
