using System.Threading.Tasks;

namespace MySeries.Application.Contracts.OmdbService
{
    public interface IOmdbSeriesService
    {
        Task<OmdbSeriesDto> GetByImdbIdAsync(string imdbId);

        // 👇 Nuevo método
        Task<OmdbSeriesSearchDto> SearchByTitleAsync(string title);
    }
}

