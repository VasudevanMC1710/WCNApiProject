using Microsoft.AspNetCore.Mvc;
using WCNApi.Applications;
using WCNApi.Applications.Services;
using WCNApi.core.Domain.Models;

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

        [HttpGet("loginuser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDetails loginUserDetails)
        {
            try
            {
                ServiceResult<LoginUserDetails> result = await _userService.LoginUser(loginUserDetails.Username, loginUserDetails.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("loginhistory")]
        public async Task<IActionResult> LoginHistory([FromBody] LoginHistory loginHistory)
        {
            try
            {
                ServiceResult<LoginHistory> result = await _userService.LoginHistory(loginHistory.Userid, loginHistory.Username, loginHistory.IpAddress);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


    }
}
