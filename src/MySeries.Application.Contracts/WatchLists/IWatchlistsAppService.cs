using MySeries.SerieService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Watchlists
{
    public interface IWatchlistsAppService : IApplicationService
    {
        Task AddSeriesFromApiAsync(string imdbId, int userId);
        Task RemoveSeriesAsync(int seriesId, int userId);
        Task<ICollection<WatchlistSerieDto>> GetWatchlistAsync(int userId);
    }
}