using System.Text.Json.Serialization;

namespace MySeries.SerieService
{
    public class serieDto
    {
        [JsonPropertyName("imdbID")]
        public string ImdbId { get; set; } = default!; // imdbID in OMDb

        [JsonPropertyName("Title")]
        public string Title { get; set; } = default!;

        [JsonPropertyName("Year")]
        public string Year { get; set; } = default!; 

        [JsonPropertyName("Genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("Plot")]
        public string? Plot { get; set; }

        [JsonPropertyName("Country")]
        public string? Country { get; set; }

        [JsonPropertyName("Poster")]
        public string? Poster { get; set; }

        [JsonPropertyName("imdbRating")]
        public string? ImdbRating { get; set; }

        [JsonPropertyName("totalSeasons")]
        public string? TotalSeasons { get; set; }

        // campos adicionales para cumplir con el punto 2
        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }

        [JsonPropertyName("Actors")]
        public string? Actors { get; set; }

        [JsonPropertyName("Director")]
        public string? Director { get; set; }

        [JsonPropertyName("Writer")]
        public string? Writer { get; set; }
    }
}
