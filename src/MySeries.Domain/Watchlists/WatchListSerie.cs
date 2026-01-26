using MySeries;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    public class WatchListSerie : Entity
    {
        public int WatchListId { get; set; }
        public int SerieId { get; set; }

        // Navegación
        public WatchList WatchList { get; set; } = null!;
        public Serie Serie { get; set; } = null!;

        protected WatchListSerie() { }

        public WatchListSerie(int watchListId, int serieId)
        {
            WatchListId = watchListId;
            SerieId = serieId;
        }

        public override object[] GetKeys()
        {
            return new object[] { WatchListId, SerieId };
        }
    }
}