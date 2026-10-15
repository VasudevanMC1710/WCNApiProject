using WCNApi.Applications.Common;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.Applications.Services.Interfaces
{
    public interface IProspectService
    {
        Task<ServiceResult<ProspectDetailsDto>> GetProspectByIdAsync(int pId);
        Task<ServiceResult<List<ProspectSearchResponse>>> SearchProspectsAsync(ProspectSearchRequest request);
    }
}
