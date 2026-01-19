using MySeries.Series;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace MySeries.Watchlists
{
    public class WatchList : AggregateRoot<int>
    {
        public Guid? UserId { get; set; }
        public List<Serie> SeriesList { get; set; }

        // Constructor EF core
        protected WatchList() { }
        
        
        
        // The EF Core protected constructor is sufficient for entity framework usage.
        // The domain constructor should accept a userId parameter for proper initialization.

        public WatchList(int userId)
        {
            UserId = userId;
            SeriesList = new List<Serie>();
        }
    }
}