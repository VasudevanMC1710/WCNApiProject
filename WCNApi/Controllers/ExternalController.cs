using Microsoft.AspNetCore.Mvc;
using WCNApi.Applications.Services.Interfaces;
using WCNApi.core.Domain.DTOs;
using WCNApi.Infrastructure.Helpers;

namespace WCNApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : Controller
    {
        private readonly ILogger<ExternalController> _logger;
        private readonly IUserService _userService;

        public ExternalController(ILogger<ExternalController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("test-decrypt")]
        public IActionResult TestDecrypt(string encryptedData, string encryptedTimestamp)
        {
            string timestamp = CryptoHelper.DecryptData(encryptedTimestamp);
            string key = CryptoHelper.GenerateSecretKey(timestamp);
            string decryptedResponse = CryptoHelper.DecryptResponse(encryptedData, key);

            return Ok(new
            {
                Timestamp = timestamp,
                DecryptedResponse = decryptedResponse
            });
        }
    }
}
