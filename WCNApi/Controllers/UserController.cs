using Microsoft.AspNetCore.Mvc;
using WCNApi.Applications.Common;
using WCNApi.Applications.Services.Interfaces;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;
using WCNApi.Globals;

namespace WCNApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("loginuser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDetails loginUserDetails)
        {
            ServiceResult<LoginUserDetails> result = await _userService.LoginUser(loginUserDetails.Username, loginUserDetails.Password);
            return Ok(result);
        }

        [HttpPost("loginhistory")]
        public async Task<IActionResult> LoginHistory([FromBody] LoginHistory loginHistory)
        {
            try
            {
                ServiceResult<int> result = await _userService.LoginHistory(loginHistory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("loginhistory/{uid:int}")]
        //public async Task<IActionResult> LoginHistory(int uid)
        //{
        //    try
        //    {
        //        ServiceResult<List<LoginHistory>> result = await _userService.LoginHistory(uid);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateUser([FromBody] AddOrUpdateUserRequestDto request)
        {

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                request.Password = Utils.EncryptPassword(request.Password);
            }

            var result = await _userService.AddOrUpdateUserAsync(request);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("user/{userId:int}/login-history")]
        public async Task<IActionResult> GetUserWithLoginHistory(int userId)
        {
            var result = await _userService.GetUserWithLoginHistoryAsync(userId);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("logo")]
        public Task<IActionResult> GetLogo()
        {
            var result = _userService.GetLogoUrl();

            if (!result.Success)
            {
                return Task.FromResult<IActionResult>(StatusCode((int)result.StatusCode, result));
            }

            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpGet("UserDashboard")]
        public async Task<IActionResult> GetUserDashboard()
        {
            var result = await _userService.GetGetUserDashboardAsync();

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
