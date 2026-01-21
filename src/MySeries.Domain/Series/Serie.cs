using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series
{
    public class Serie : AggregateRoot<int>
    {
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public string? Plot { get; set; }
        public required string Year { get; set; } 
        public string? Country { get; set; }
        public string? ImdbId { get; set; } 
        public string? ImdbRating { get; set; }
        public string? TotalSeasons { get; set; } 
        public string? Poster { get; set; }     
        public string? Runtime { get; set; }    
        public string? Actors { get; set; }     
        public string? Director { get; set; }    
        public string? Writer { get; set; }     

    }
}
