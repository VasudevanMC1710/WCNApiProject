using WCNApi.core.Domain.Models;
using WCNApi.core.Domain.Repositories;
using WCNApi.Globals;

namespace WCNApi.Applications.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginUserDetails>> LoginUser(string userName,string password)
        {
            LoginUserDetails userDetails = await _userRepository.LoginUser(userName, Utils.EncryptPassword(password)); 
            return ServiceResult<LoginUserDetails>.SuccessResult(userDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginHistory>> LoginHistory(int userId, string userName,string ipAddress)
        {
            LoginHistory userDetails = await _userRepository.LoginHistory(userId,userName, ipAddress);
            return ServiceResult<LoginHistory>.SuccessResult(userDetails);
        }
    }
}
