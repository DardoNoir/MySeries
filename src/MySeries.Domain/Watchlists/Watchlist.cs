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

        // Constructor de Dominio
        public WatchList(Guid UserId)
        {
            SeriesList = new List<Serie>();
        }

    }
}