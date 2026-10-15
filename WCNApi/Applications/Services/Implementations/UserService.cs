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
        public async Task<ServiceResult<LoginUserDetails>> LoginUser(string userName, string password)
        {
            _logger.LogInformation("LoginUser started.");

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning("LoginUser failed: Username is required.");
                return ServiceResult<LoginUserDetails>.ErrorResult(
                    "Username is required.", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("LoginUser failed: Password is required.");
                return ServiceResult<LoginUserDetails>.ErrorResult(
                    "Password is required.", HttpStatusCode.BadRequest);
            }

            try
            {
                string encryptedPassword = Utils.EncryptPassword(password);

                LoginUserDetails? userDetails = await _userRepository.LoginUser(
                    userName.Trim(), encryptedPassword);

                if (userDetails == null)
                {
                    _logger.LogWarning("LoginUser failed: Invalid credentials.");
                    return ServiceResult<LoginUserDetails>.ErrorResult(
                        "Invalid username or password.", HttpStatusCode.Unauthorized);
                }

                if (userDetails.Active != BusinessGlobals.ActiveStatus)
                {
                    _logger.LogWarning("LoginUser failed: Inactive user. UserId: {UserId}", userDetails.Uid);
                    return ServiceResult<LoginUserDetails>.ErrorResult(
                        "User is inactive.", HttpStatusCode.Unauthorized);
                }

                _logger.LogInformation("LoginUser succeeded. UserId: {UserId}", userDetails.Uid);

                return ServiceResult<LoginUserDetails>.SuccessResult(userDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginUser failed due to an unexpected error.");
                return ServiceResult<LoginUserDetails>.ErrorResult("An error occurred during login.", 
                    HttpStatusCode.InternalServerError);
            }
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
            _logger.LogInformation("LoginHistory started.");

            try
            {
                int createdId = await _userRepository.LoginHistory(loginHistory);

                _logger.LogInformation("LoginHistory created successfully. HistoryId: {HistoryId}", createdId);
                return ServiceResult<int>.SuccessResult(createdId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginHistory failed.");
                return ServiceResult<int>.ErrorResult("An error occurred while saving login history.",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<LoginUserDetailsModel>> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation("GetUserByIdAsync started. UserId: {UserId}", userId);

            if (userId <= 0)
            {
                _logger.LogWarning("GetUserByIdAsync failed: Invalid UserId: {UserId}", userId);
                return ServiceResult<LoginUserDetailsModel>.ErrorResult(
                    "Invalid User Id.", HttpStatusCode.BadRequest);
            }

            try
            {
                LoginUserDetailsModel? user = await _userRepository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("GetUserByIdAsync failed: User not found. UserId: {UserId}", userId);
                    return ServiceResult<LoginUserDetailsModel>.ErrorResult(
                        "User not found.", HttpStatusCode.NotFound);
                }

                _logger.LogInformation("GetUserByIdAsync succeeded. UserId: {UserId}", userId);
                return ServiceResult<LoginUserDetailsModel>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserByIdAsync failed. UserId: {UserId}", userId);
                return ServiceResult<LoginUserDetailsModel>.ErrorResult("An error occurred while retrieving the user.", 
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<bool>> AddOrUpdateUserAsync(AddOrUpdateUserRequestDto request)
        {
            _logger.LogInformation("AddOrUpdateUserAsync started. UserId: {UserId}", request.Uid);

            if (request.Uid < 0)
            {
                _logger.LogWarning("AddOrUpdateUserAsync failed: Invalid UserId: {UserId}", request.Uid);
                return ServiceResult<bool>.ErrorResult("Invalid User Id.", HttpStatusCode.BadRequest);
            }

            try
            {
                bool addOrUpdated = await _userRepository.AddOrUpdateUserAsync(request);

                if (!addOrUpdated)
                {
                    _logger.LogWarning("AddOrUpdateUserAsync failed: User not found. UserId: {UserId}", request.Uid);
                    return ServiceResult<bool>.ErrorResult("User not found.", HttpStatusCode.NotFound);
                }

                _logger.LogInformation("AddOrUpdateUserAsync succeeded. Operation: {Operation}, UserId: {UserId}",
                    request.Uid == 0 ? "Add" : "Update", request.Uid);
                return ServiceResult<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddOrUpdateUserAsync failed. UserId: {UserId}", request.Uid);
                return ServiceResult<bool>.ErrorResult("An error occurred while saving the user.",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<UserWithLoginHistoryDto>> GetUserWithLoginHistoryAsync(int userId)
        {
            _logger.LogInformation("GetUserWithLoginHistoryAsync started. UserId: {UserId}", userId);

            if (userId <= 0)
            {
                _logger.LogWarning("GetUserWithLoginHistoryAsync failed: Invalid UserId: {UserId}", userId);
                return ServiceResult<UserWithLoginHistoryDto>.ErrorResult(
                    "Invalid User Id.", HttpStatusCode.BadRequest);
            }

            try
            {
                var result = await _userRepository.GetUserWithLoginHistoryAsync(userId);

                if (result == null)
                {
                    _logger.LogWarning("GetUserWithLoginHistoryAsync failed: User not found. UserId: {UserId}", userId);
                    return ServiceResult<UserWithLoginHistoryDto>.ErrorResult(
                        "User not found.", HttpStatusCode.NotFound);
                }

                _logger.LogInformation("GetUserWithLoginHistoryAsync succeeded. UserId: {UserId}", userId);
                return ServiceResult<UserWithLoginHistoryDto>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserWithLoginHistoryAsync failed. UserId: {UserId}", userId);
                return ServiceResult<UserWithLoginHistoryDto>.ErrorResult(
                    "An error occurred while retrieving user login history.",
                    HttpStatusCode.InternalServerError);
            }
        }

        public ServiceResult<string> GetLogoUrl()
        {
            _logger.LogInformation("GetLogoUrl started.");

            if (string.IsNullOrWhiteSpace(BusinessGlobals.LogoUrl))
            {
                _logger.LogWarning("GetLogoUrl failed: Logo URL is not configured.");
                return ServiceResult<string>.ErrorResult("Logo URL is not configured.", 
                    HttpStatusCode.NotFound);
            }

            _logger.LogInformation("GetLogoUrl succeeded.");
            return ServiceResult<string>.SuccessResult(BusinessGlobals.LogoUrl);
        }

        public async Task<ServiceResult<UserDashboardDto>> GetGetUserDashboardAsync()
        {
            _logger.LogInformation("GetGetUserDashboardAsync started.");

            try
            {
                var result = await _userRepository.GetUserDashboardAsync();

                if (result == null)
                {
                    _logger.LogWarning("GetGetUserDashboardAsync failed: Dashboard not found.");
                    return ServiceResult<UserDashboardDto>.ErrorResult("User dashboard not found.", 
                        HttpStatusCode.NotFound);
                }

                _logger.LogInformation("GetGetUserDashboardAsync succeeded.");
                return ServiceResult<UserDashboardDto>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGetUserDashboardAsync failed.");
                return ServiceResult<UserDashboardDto>.ErrorResult(
                    "An error occurred while retrieving the user dashboard.",
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}
