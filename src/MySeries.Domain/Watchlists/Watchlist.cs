using MySeries.Series;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace MySeries.Watchlists
{
    public class WatchList : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public ICollection<WatchListSerie> Series { get; set; }
        
        protected WatchList() { }


        public WatchList(int userId)
        {
            UserId = userId;
            Series = new List<WatchListSerie>();
        }
    }
}