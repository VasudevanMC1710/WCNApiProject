using System.Net;
using WCNApi.Applications.Common;
using WCNApi.Applications.Services.Interfaces;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;
using WCNApi.core.Domain.Repositories;

namespace WCNApi.Applications.Services.Implementations
{
    public class ProspectService : IProspectService
    {
        private readonly IProspectRepository _prospectRepository;
        private readonly ILogger<ProspectService> _logger;

        public ProspectService(IProspectRepository prospectRepository, 
            ILogger<ProspectService> logger)
        {
            _prospectRepository = prospectRepository;
            _logger = logger;
        }

        public async Task<ServiceResult<ProspectDetailsDto>> GetProspectByIdAsync(int pId)
        {
            _logger.LogInformation("GetProspectByIdAsync started. ProspectId: {ProspectId}", pId);

            if (pId <= 0)
            {
                _logger.LogWarning("GetProspectByIdAsync failed: Invalid ProspectId: {ProspectId}", pId);
                return ServiceResult<ProspectDetailsDto>.ErrorResult("Invalid Prospect Id.",
                    HttpStatusCode.BadRequest);
            }

            try
            {
                var prospect = await _prospectRepository.GetProspectByIdAsync(pId);

                if (prospect == null)
                {
                    _logger.LogWarning("GetProspectByIdAsync failed: Prospect not found. ProspectId: {ProspectId}", pId);
                    return ServiceResult<ProspectDetailsDto>.ErrorResult("Prospect not found.", HttpStatusCode.NotFound);
                }

                _logger.LogInformation("GetProspectByIdAsync succeeded. ProspectId: {ProspectId}", pId);
                return ServiceResult<ProspectDetailsDto>.SuccessResult(prospect);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProspectByIdAsync failed due to an unexpected error. ProspectId: {ProspectId}", pId);
                return ServiceResult<ProspectDetailsDto>.ErrorResult("An error occurred while retrieving the prospect.",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<List<ProspectSearchResponse>>> SearchProspectsAsync(ProspectSearchRequest request)
        {
            _logger.LogInformation("SearchProspectsAsync started.");

            if (request == null)
            {
                _logger.LogWarning("SearchProspectsAsync failed: Search request is required.");
                return ServiceResult<List<ProspectSearchResponse>>.ErrorResult(
                    "Search request is required.", HttpStatusCode.BadRequest);
            }

            if (request.FromDate.HasValue && request.ToDate.HasValue &&
                request.FromDate.Value.Date > request.ToDate.Value.Date)
            {
                _logger.LogWarning("SearchProspectsAsync failed: FromDate is greater than ToDate.");
                return ServiceResult<List<ProspectSearchResponse>>.ErrorResult(
                    "From Date cannot be greater than To Date.", HttpStatusCode.BadRequest);
            }

            try
            {
                List<ProspectSearchResponse> prospects = await _prospectRepository.SearchProspectsAsync(request);

                if (prospects == null || prospects.Count == 0)
                {
                    _logger.LogWarning("SearchProspectsAsync completed: No prospects found.");
                    return ServiceResult<List<ProspectSearchResponse>>.ErrorResult(
                        "No prospects found.", HttpStatusCode.NotFound);
                }

                _logger.LogInformation("SearchProspectsAsync succeeded. ResultCount: {ResultCount}",
                    prospects.Count);
                return ServiceResult<List<ProspectSearchResponse>>.SuccessResult(prospects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchProspectsAsync failed due to an unexpected error.");
                return ServiceResult<List<ProspectSearchResponse>>.ErrorResult(
                    "An error occurred while searching prospects.",
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}
