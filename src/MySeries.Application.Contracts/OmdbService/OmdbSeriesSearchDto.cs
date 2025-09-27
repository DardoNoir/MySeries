using System.Collections.Generic;

namespace MySeries.Application.Contracts.OmdbService
{
    public class OmdbSeriesSearchDto
    {
        public List<OmdbSeriesSearchItemDto> Search { get; set; } = new();
        public string? TotalResults { get; set; }
        public string? Response { get; set; }
    }

    public class OmdbSeriesSearchItemDto
    {
        public string ImdbId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Year { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Poster { get; set; }
        public string? Genre { get; set; } 
    }
}
