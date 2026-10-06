using WCNApi.core.Domain.Models;

namespace WCNApi.Applications.Services
{
    public interface IUserService
    {
        Task<ServiceResult<LoginUserDetails>> LoginUser(string userName, string password);
        Task<ServiceResult<LoginHistory>> LoginHistory(int userId, string userName, string ipAddress);
    }
}
