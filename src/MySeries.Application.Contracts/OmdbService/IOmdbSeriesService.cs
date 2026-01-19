using MySeries.SerieService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySeries.Application.Contracts.OmdbService
{
    public interface IOmdbSeriesService
    {
        Task<SerieDto> GetByImdbIdAsync(string imdbId);
        Task<ICollection<SerieDto>> SearchByTitleAsync(string title);
    }
}

