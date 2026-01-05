using MySeries.SerieService;
using System.Threading.Tasks;

namespace MySeries.Application.Contracts.OmdbService
{
    public interface IOmdbSeriesService
    {
        Task<serieDto> GetByImdbIdAsync(string imdbId);
        Task<OmdbSeriesSearchDto> SearchByTitleAsync(string title);
    }
}

