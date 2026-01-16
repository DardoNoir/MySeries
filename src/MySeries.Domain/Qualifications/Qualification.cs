using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;
using Volo.Abp.Domain.Entities;

namespace MySeries.Qualifications
{
    public class Qualification : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public int SerieId { get; set; }
        public int Score { get; set; } // Puntuación del 1 al 10
        public string? Review { get; set; } // Comentario opcional

    }
}