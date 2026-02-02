using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class OmdbSearchResponse
    {
        [JsonProperty("Search")]
        public List<OmdbSerieItems>? Search {get; set;}
    }

    // Items que devuelve la búsqueda por Título
    public class OmdbSerieItems
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? ImdbId { get; set; }
        public string? Poster {  get; set; }
        public string? Type { get; set; }
    }

    // Items extra que devuelve la búsqueda por ImdbId
    public class OmdbDetailResponse
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? ImdbId { get; set; }
        public string? Poster {  get; set; }

        public string? Genre { get; set; }
        public string? Plot { get; set; }
        public string? Country { get; set; }
        public string? ImdbRating { get; set; }
        public string? TotalSeasons { get; set; } 
        public string? Runtime { get; set; }    
        public string? Actors { get; set; }     
        public string? Director { get; set; }    
        public string? Writer { get; set; }     
    }
}

