using MySeries.SerieService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySeries.Application.Contracts.OmdbService
{
    public interface IOmdbSeriesService
    {
        Task<serieDto> GetByImdbIdAsync(string imdbId);
        Task<ICollection<serieDto>> SearchByTitleAsync(string title);
    }
}

