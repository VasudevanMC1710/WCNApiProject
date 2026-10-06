using WCNApi.core.Domain.Models;

namespace WCNApi.core.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<LoginUserDetails> LoginUser(string userName, string password);
        Task<LoginHistory> LoginHistory(int userId, string userName, string ipAddress);
    }
}
