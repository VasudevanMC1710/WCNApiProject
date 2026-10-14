using WCNApi.Applications.Common;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.Applications.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<LoginUserDetails>> LoginUser(string userName, string password);
        Task<ServiceResult<int>> LoginHistory(LoginHistory loginHistory);
        Task<ServiceResult<LoginUserDetailsModel>> GetUserByIdAsync(int userId);
        Task<ServiceResult<bool>> UpdateUserAsync(int userId, UpdateUserRequestDto request);
        Task<ServiceResult<UserWithLoginHistoryDto>> GetUserWithLoginHistoryAsync(int userId);
        ServiceResult<string> GetLogoUrl();
        Task<ServiceResult<UserDashboardDto>> GetGetUserDashboardAsync();
    }
}
