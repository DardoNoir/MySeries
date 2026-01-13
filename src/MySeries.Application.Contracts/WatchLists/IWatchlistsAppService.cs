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
        Task AddSeriesAsync(Guid seriesId);
        Task RemoveSeriesAsync(Guid seriesId);
        Task<ICollection<SerieDto>> GetWatchlistAsync();
    }
}