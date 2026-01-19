using MySeries.SerieService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.WatchLists
{
    public interface IWatchlistsAppService : IApplicationService
    {
        Task AddSeriesAsync(int seriesId, int userId);
        Task RemoveSeriesAsync(int seriesId, int userId);
        Task<ICollection<SerieDto>> GetWatchlistAsync(int userId);
    }
}