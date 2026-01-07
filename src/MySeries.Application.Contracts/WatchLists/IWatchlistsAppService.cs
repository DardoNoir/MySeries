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
        // task AddSeriesAsync(int seriesId, int UserId);
        // task RemoveSeriesAsync(int seriesId, int UserId);
        // task<ICollection<SeriesDto>> GetWatchlistAsync(int UserId);
    }
}