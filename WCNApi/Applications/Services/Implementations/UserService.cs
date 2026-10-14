using System.Net;
using WCNApi.Applications.Common;
using WCNApi.Applications.Services.Interfaces;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;
using WCNApi.core.Domain.Repositories;
using WCNApi.Globals;

namespace WCNApi.Applications.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginUserDetails>> LoginUser(string userName,string password)
        {
            _logger.LogInformation("STEP:[1] | LoginUser | Login process started.");

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning("STEP:[2] | LoginUser | Login validation failed: Username is required.");

                return ServiceResult<LoginUserDetails>.ErrorResult("Username is required.", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("STEP:[3] | LoginUser | Login validation failed: Password is required.");

                return ServiceResult<LoginUserDetails>.ErrorResult("Password is required.", HttpStatusCode.BadRequest);
            }

            _logger.LogInformation("STEP:[4] | LoginUser | Login request validation completed.");

            string encryptedPassword = Utils.EncryptPassword(password);

            _logger.LogInformation("STEP:[5] | LoginUser | Password encryption completed.");

            LoginUserDetails? userDetails = await _userRepository.LoginUser(userName.Trim(), encryptedPassword);

            // Invalid credentials
            if (userDetails == null)
            {
                _logger.LogWarning("STEP:[6] | LoginUser | Login failed: Invalid username or password.");
                return ServiceResult<LoginUserDetails>.ErrorResult("Invalid username or password.", HttpStatusCode.Unauthorized);
            }
            if (userDetails.Active != BusinessGlobals.ActiveStatus)
            {
                _logger.LogWarning("STEP:[7] | LoginUser | Login failed: User is inactive. UserId: {UserId}", userDetails.Uid);
                return ServiceResult<LoginUserDetails>.ErrorResult("User is inactive.", HttpStatusCode.Unauthorized);
            }

            _logger.LogInformation("STEP:[8] | LoginUser | Login successful. UserId: {UserId}", userDetails.Uid);
            return ServiceResult<LoginUserDetails>.SuccessResult(userDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> LoginHistory(LoginHistory loginHistory)
        {
            int createdId = await _userRepository.LoginHistory(loginHistory);
            return ServiceResult<int>.SuccessResult(createdId);
        }

        public async Task<ServiceResult<LoginUserDetailsModel>>GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                return ServiceResult<LoginUserDetailsModel>.ErrorResult("Invalid User Id.",
                    HttpStatusCode.BadRequest);
            }

            LoginUserDetailsModel? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<LoginUserDetailsModel>.ErrorResult("User not found.", HttpStatusCode.NotFound);
            }

            return ServiceResult<LoginUserDetailsModel>.SuccessResult(user);
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(int userId, UpdateUserRequestDto request)
        {
            if (userId <= 0)
            {
                return ServiceResult<bool>.ErrorResult("Invalid User Id.", HttpStatusCode.BadRequest);
            }

            bool updated = await _userRepository.UpdateUserAsync(userId,
                request.FirstName, request.LastName, request.Active);

            if (!updated)
            {
                return ServiceResult<bool>.ErrorResult("User not found.", HttpStatusCode.NotFound);
            }

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<UserWithLoginHistoryDto>>GetUserWithLoginHistoryAsync(int userId)
        {
            if (userId <= 0)
            {
                return ServiceResult<UserWithLoginHistoryDto>
                    .ErrorResult(
                        "Invalid User Id.",
                        HttpStatusCode.BadRequest);
            }

            var result =
                await _userRepository
                    .GetUserWithLoginHistoryAsync(userId);

            if (result == null)
            {
                return ServiceResult<UserWithLoginHistoryDto>.ErrorResult("User not found.",
                        HttpStatusCode.NotFound);
            }

            return ServiceResult<UserWithLoginHistoryDto>
                .SuccessResult(result);
        }

        public ServiceResult<string> GetLogoUrl()
        {
            if (string.IsNullOrWhiteSpace(BusinessGlobals.LogoUrl))
            {
                return ServiceResult<string>.ErrorResult("Logo URL is not configured.", HttpStatusCode.NotFound);
            }

            return ServiceResult<string>.SuccessResult(BusinessGlobals.LogoUrl);
        }

        public async Task<ServiceResult<UserDashboardDto>> GetGetUserDashboardAsync()
        {
            
            var result = await _userRepository.GetUserDashboardAsync();

            if (result == null)
            {
                return ServiceResult<UserDashboardDto>.ErrorResult("User dashboard not found.",
                        HttpStatusCode.NotFound);
            }

            return ServiceResult<UserDashboardDto>.SuccessResult(result);
        }
    }
}
