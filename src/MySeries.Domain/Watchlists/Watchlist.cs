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
        
        
        
        // Constructor de Dominio
        public WatchList(Guid idUsuario)
        {
            UserId = idUsuario;
            SeriesList = new List<Serie>();
        }

    }
}