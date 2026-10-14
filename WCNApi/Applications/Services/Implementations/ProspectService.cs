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
        public ProspectService(IProspectRepository prospectRepository)
        {
            _prospectRepository = prospectRepository;
        }

        public async Task<ServiceResult<ProspectDetailsDto>>GetProspectByIdAsync(int pId)
        {
            if (pId <= 0)
            {
                return ServiceResult<ProspectDetailsDto>.ErrorResult("Invalid Prospect Id.", HttpStatusCode.BadRequest);
            }

            var prospect = await _prospectRepository.GetProspectByIdAsync(pId);

            if (prospect == null)
            {
                return ServiceResult<ProspectDetailsDto>.ErrorResult("Prospect not found.", HttpStatusCode.NotFound);
            }

            return ServiceResult<ProspectDetailsDto>.SuccessResult(prospect);
        }

        public async Task<ServiceResult<List<ProspectSearchResponse>>>SearchProspectsAsync(
        ProspectSearchRequest request)
        {
            if (request == null)
            {
                return ServiceResult<
                    List<ProspectSearchResponse>>
                    .ErrorResult(
                        "Search request is required.",
                        HttpStatusCode.BadRequest);
            }

            if (request.FromDate.HasValue &&
                request.ToDate.HasValue &&
                request.FromDate.Value.Date >
                request.ToDate.Value.Date)
            {
                return ServiceResult<
                    List<ProspectSearchResponse>>
                    .ErrorResult(
                        "From Date cannot be greater than To Date.",
                        HttpStatusCode.BadRequest);
            }

            List<ProspectSearchResponse> prospects =
                await _prospectRepository
                    .SearchProspectsAsync(request);

            if (prospects == null ||
                prospects.Count == 0)
            {
                return ServiceResult<
                    List<ProspectSearchResponse>>
                    .ErrorResult(
                        "No prospects found.",
                        HttpStatusCode.NotFound);
            }

            return ServiceResult<
                List<ProspectSearchResponse>>
                .SuccessResult(prospects);
        }
    }
}
