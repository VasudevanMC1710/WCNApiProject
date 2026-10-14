using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;

namespace WCNApi.core.Domain.Repositories
{
    public interface IProspectRepository
    {
        Task<ProspectDetailsDto?> GetProspectByIdAsync(int pId);
        Task<List<ProspectSearchResponse>> SearchProspectsAsync(ProspectSearchRequest request);
    }
}
