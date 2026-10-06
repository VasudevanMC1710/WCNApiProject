using Microsoft.Data.SqlClient;
using WCNApi.core.Domain.Models;
using WCNApi.core.Domain.Repositories;

namespace WCNApi.core.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginUserDetails> LoginUser(string userName, string password)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@username", userName),
                new SqlParameter("@password", password)
            };
            LoginUserDetails userDetails = await _context.ExecuteStoreProcedureSingleAsync<LoginUserDetails>(StoreProcedures.sp_CheckLoginCredinitals, sqlParameters);
            return userDetails;
        }

        public async Task<LoginHistory> LoginHistory(int userId,string userName, string ipAddress)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Userid", userId),
                new SqlParameter("@Username", userName),
                new SqlParameter("@IpAddress", ipAddress)
            };
            LoginHistory loginHistory = await _context.ExecuteStoreProcedureSingleAsync<LoginHistory>(StoreProcedures.sp_LoginHistory, sqlParameters);
            return loginHistory;
        }
    }
}
