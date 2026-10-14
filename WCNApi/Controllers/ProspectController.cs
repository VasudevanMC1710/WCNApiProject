using Microsoft.AspNetCore.Mvc;
using WCNApi.Applications.Common;
using WCNApi.Applications.Services.Interfaces;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectController : ControllerBase
    {
        private readonly IProspectService _prospectService;

        public ProspectController(IProspectService prospectService)
        {
            _prospectService = prospectService;
        }

        [HttpGet("{pId:int}")]
        public async Task<IActionResult>GetProspectById(int pId)
        {
            var result = await _prospectService.GetProspectByIdAsync(pId);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult>SearchProspects([FromBody] ProspectSearchRequest request)
        {
            ServiceResult<List<ProspectSearchResponse>> result = await 
                _prospectService.SearchProspectsAsync(request);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
