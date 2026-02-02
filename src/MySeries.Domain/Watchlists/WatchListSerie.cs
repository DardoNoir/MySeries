using MySeries;
using Volo.Abp.Domain.Entities;

namespace MySeries.Watchlists
{
    // Entidad que representa la relación entre las Series y las Watchlists
    public class WatchListSerie : Entity
    {
        public int WatchListId { get; set; }
        public int SerieId { get; set; }

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