using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    public class WatchList : Entity<int>
    {
        public int UserId { get;  set; }
        public ICollection<WatchListSerie> WatchListSeries { get;  set; }

        // Constructor para EF Core
        protected WatchList()
        {
            WatchListSeries = new List<WatchListSerie>();
        }

        // Constructor de dominio
        public WatchList(int userId)
        {
            UserId = userId;
            WatchListSeries = new List<WatchListSerie>();
        }
    }
}