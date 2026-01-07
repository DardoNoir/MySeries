using System;
using System.Collections.Generic;
using MySeries.Series;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    public class WatchList : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public List<Serie> SeriesList { get; set; }

        public WatchList()
        {
            SeriesList = new List<Serie>();
        }

    }
}