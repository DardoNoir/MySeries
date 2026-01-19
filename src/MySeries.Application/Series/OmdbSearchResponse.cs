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

    public class OmdbSerieItems
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbId { get; set; }
        public string Poster {  get; set; }
        public string Type { get; set; }
    }
}
