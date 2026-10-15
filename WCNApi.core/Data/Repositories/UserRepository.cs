using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                new SqlParameter("@Network", loginHistory.Network),
                new SqlParameter("@Connectiontype", loginHistory.Connectiontype),
                new SqlParameter("@City", loginHistory.City),
                new SqlParameter("@UserType", loginHistory.UserType),
                new SqlParameter("@ISP", loginHistory.ISP),
                new SqlParameter("@TimeZone", loginHistory.TimeZone),
                new SqlParameter("@PostalCode", loginHistory.PostalCode),
                new SqlParameter("@Country", loginHistory.Country),
                new SqlParameter("@Active", loginHistory.Active)
            };
            var result = await _context.ExecuteStoreProcedureSingleAsync<int>(StoreProcedures.sp_Add_LoginHistory_Dig, sqlParameters);
            return result;
        }

        public async Task<LoginUserDetailsModel?> GetUserByIdAsync(int userId)
        {
            return await _context.LoginUsers.AsNoTracking().FirstOrDefaultAsync(
                    x => x.Uid == userId);
        }



        public async Task<bool> AddOrUpdateUserAsync(AddOrUpdateUserRequestDto request)
        {
            if (request.Uid == 0)
            {
                // Add new user
                var newUser = _mapper.Map<LoginUserDetailsModel>(request);
                _context.LoginUsers.Add(newUser);
                await _context.SaveChangesAsync();
                return true;
            }

            // Find existing user
            var user = await _context.LoginUsers.FirstOrDefaultAsync(x => x.Uid == request.Uid);

            if (user == null)
            {
                return false;
            }

            // Update all mapped columns on the tracked entity
            _mapper.Map(request, user);
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
