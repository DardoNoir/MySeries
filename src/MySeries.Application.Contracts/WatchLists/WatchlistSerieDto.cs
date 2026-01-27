using Volo.Abp.Application.Dtos;

namespace MySeries.Watchlists
{
    public class WatchlistSerieDto : EntityDto<int>
    {
     //   public int SerieId { get; set; }
        public string Title { get; set; } = null!;
        public string? Poster { get; set; }
        public string? Genre { get; set; }
        public string Year { get; set; } = null!;
        public string ImdbId { get; set; } = null!;

        // 🔥 Calificación del usuario
        public int? Score { get; set; }
        public string? Review { get; set; }
    }
}
