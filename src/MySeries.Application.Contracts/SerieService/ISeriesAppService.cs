using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using MySeries.Application.Contracts.OmdbService;

namespace MySeries.Application.Contracts
{
    public interface ISeriesAppService : IApplicationService
    {
        Task<OmdbSeriesDto> GetFromOmdbAsync(string imdbId);
    }
}
