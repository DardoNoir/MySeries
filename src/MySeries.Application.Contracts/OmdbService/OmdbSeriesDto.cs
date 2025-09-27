
namespace MySeries.Application.Contracts.OmdbService
{
     public class OmdbSeriesDto
    {
        public string ImdbId { get; set; } = default!; // imdbID
        public string Title { get; set; } = default!;
        public string Year { get; set; } = default!;
        public string? Genre { get; set; }
        public string? Plot { get; set; } // descripción
        public string? Country { get; set; }
        public string? Poster { get; set; }
        public string? ImdbRating { get; set; }
        public string? TotalSeasons { get; set; }
    }
}