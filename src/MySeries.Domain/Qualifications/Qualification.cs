using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using Volo.Abp.Domain.Entities;

namespace MySeries.Qualifications
{
    public class Qualification : AggregateRoot<int>
    {
        public Guid? UserId { get; set; }
        public Guid? SerieId { get; set; }
        public int Score { get; set; } // Puntuación del 1 al 10
        public string? Review { get; set; } // Comentario opcional

        // Constuctor de Dominio
        public Qualification(Guid uIs, Guid sId, int scr, string? rev)
        {
            UserId = uIs;
            SerieId = sId;
            Score = scr;
            Review = rev;
        }

    }
}