using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    public class WatchList : Entity<int>
    {
        public int UserId { get; protected set; }

        // ✅ Relación many-to-many vía tabla intermedia
        public ICollection<WatchListSerie> WatchListSeries { get; protected set; }

        protected WatchList()
        {
            WatchListSeries = new List<WatchListSerie>();
        }

        public WatchList(int userId)
        {
            UserId = userId;
            WatchListSeries = new List<WatchListSerie>();
        }
    }
}