using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WCNApi.core.Domain.DTOs;
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

        public async Task<LoginUserDetails?> LoginUser(string userName, string password)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@username", userName),
                new SqlParameter("@password", password)
            };
            LoginUserDetails? userDetails = await _context.ExecuteStoreProcedureSingleAsync<LoginUserDetails>(
                StoreProcedures.sp_CheckLoginCredinitals_Dig, sqlParameters);
            return userDetails;
        }

        public async Task<int> LoginHistory(LoginHistory loginHistory)  
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Userid", loginHistory.Userid),
                new SqlParameter("@Username", loginHistory.Username),
                new SqlParameter("@IpAddress", loginHistory.IpAddress),
                new SqlParameter("@MachineName", loginHistory.MachineName),
                new SqlParameter("@Status", loginHistory.Status),
                new SqlParameter("@Reason", loginHistory.Reason),
                new SqlParameter("@City", loginHistory.City),
                new SqlParameter("@Region", loginHistory.Region),
                new SqlParameter("@Pincode", loginHistory.Pincode),
                new SqlParameter("@Country", loginHistory.Country),
                new SqlParameter("@Active", loginHistory.Active)
            };
            var result = await _context.ExecuteNonQueryAsync<int>(StoreProcedures.sp_Add_LoginHistory_Dig, sqlParameters);
            return result;
        }

        public async Task<LoginUserDetailsModel?> GetUserByIdAsync(int userId)
        {
            return await _context.LoginUsers.AsNoTracking().FirstOrDefaultAsync(
                    x => x.Uid == userId);
        }

        public async Task<bool> UpdateUserAsync(int userId, string firstName, string lastName, int active)
        {
            LoginUserDetails? user = await _context
                .Set<LoginUserDetails>()
                .FirstOrDefaultAsync(x => x.Uid == userId);

            if (user == null)
            {
                return false;
            }

            user.Firstname = firstName;
            user.Lastname = lastName;
            user.Active = active;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserWithLoginHistoryDto?>GetUserWithLoginHistoryAsync(int userId)
        {
            return await _context.LoginUsers
                .AsNoTracking()
                .Where(x => x.Uid == userId)
                .Select(x => new UserWithLoginHistoryDto
                {
                    Uid = x.Uid,
                    Username = x.Username,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    EmailAddress = x.EmailAddress,

                    LoginHistories = x.LoginHistories
                        .OrderByDescending(h => h.LoginDate)
                        .Select(h => new LoginHistoryDto
                        {
                            Lid = h.Lid,
                            LoginDate = h.LoginDate,
                            IpAddress = h.IpAddress,
                            MachineName = h.MachineName,
                            Status = h.Status,
                            City = h.City,
                            Country = h.Country
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserDashboardDto?> GetUserDashboardAsync()
        {
            return await _context.ExecuteStoreProcedureSingleAsync<UserDashboardDto>
                (StoreProcedures.sp_Get_UserDashboard_Dig);
        }
    }
}
