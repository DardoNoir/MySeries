using Microsoft.EntityFrameworkCore;
using MySeries.Series;
using MySeries.SerieService;
using MySeries.Watchlists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Watchlists
{
    public class WatchlistsAppService : ApplicationService, IWatchlistsAppService
    {
        private readonly IRepository<WatchList, int> _watchlistRepository;
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly IRepository<WatchListSerie> _watchListSerieRepository;
        private readonly SerieAppService _serieAppService;

        public WatchlistsAppService(
            IRepository<WatchList, int> watchlistRepository,
            IRepository<Serie, int> serieRepository,
            IRepository<WatchListSerie> watchListSerieRepository,
            SerieAppService serieAppService)
        {
            _watchlistRepository = watchlistRepository;
            _serieRepository = serieRepository;
            _watchListSerieRepository = watchListSerieRepository;
            _serieAppService = serieAppService;
        }

        // Agregar una serie (desde la API) a la watchlist del usuario
        [RemoteService(IsEnabled = false)]
        public async Task AddSeriesFromApiAsync(string imdbId, int userId)
        {
            // 1️⃣ Verificar que el usuario esté autenticado
            if (userId <= 0)
                throw new BusinessException("UsuarioNoAutenticado");

            // 2️⃣ Obtener o crear la serie desde la API OMDb
            var serieDto = await _serieAppService.GetOrCreateFromApiAsync(imdbId);

            // 3️⃣ Obtener la entidad Serie persistida
            var serieEntity = await _serieRepository.GetAsync(s => s.ImdbId == imdbId);

            // 4️⃣ Obtener la watchlist del usuario incluyendo sus series
            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(w => w.Series))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            // 5️⃣ Si el usuario no tiene watchlist, crearla
            if (watchlist == null)
            {
                watchlist = new WatchList(userId);
                await _watchlistRepository.InsertAsync(watchlist, autoSave: true);
            }

            // 6️⃣ Verificar si la serie ya está asociada a la watchlist
            var alreadyAdded = await _watchListSerieRepository.AnyAsync(
                x => x.WatchListId == watchlist.Id && x.SerieId == serieEntity.Id);

            if (alreadyAdded)
                return; // La serie ya está en la watchlist → no hacer nada

            // 7️⃣ Crear la relación WatchList ↔ Serie (tabla intermedia)
            var relation = new WatchListSerie(
                watchlist.Id,
                serieEntity.Id
            );

            await _watchListSerieRepository.InsertAsync(relation);
        }

        // Eliminar una serie de la watchlist del usuario
        public async Task RemoveSeriesAsync(int seriesId, int userId)
        {
            // 1️⃣ Verificar que el usuario esté autenticado
            if (userId <= 0)
                throw new BusinessException("UsuarioNoAutenticado");

            // 2️⃣ Obtener la watchlist del usuario
            var watchlist = await _watchlistRepository
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
                throw new BusinessException("ListaDeSeguimientoNoEncontrada");

            // 3️⃣ Buscar la relación WatchListSerie
            var relation = await _watchListSerieRepository.FirstOrDefaultAsync(
                x => x.WatchListId == watchlist.Id && x.SerieId == seriesId);

            if (relation == null)
                throw new BusinessException("SerieNoEncontradaEnLaWatchlist");

            // 4️⃣ Eliminar la relación (NO la serie)
            await _watchListSerieRepository.DeleteAsync(relation);
        }

        // Obtener la watchlist del usuario
        public async Task<ICollection<SerieDto>> GetWatchlistAsync(int userId)
        {
            // 1️⃣ Verificar que el usuario esté autenticado
            if (userId <= 0)
                throw new BusinessException("UsuarioNoAutenticado");

            // 2️⃣ Obtener la watchlist con las series asociadas
            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(
                    w => w.Series,
                    w => w.Series.Select(ws => ws.Serie)))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null || !watchlist.Series.Any())
                return new List<SerieDto>();

            // 3️⃣ Mapear las series a SerieDto
            return watchlist.Series.Select(ws => new SerieDto
            {
                ImdbId = ws.Serie.ImdbId,
                Title = ws.Serie.Title,
                Year = ws.Serie.Year,
                Genre = ws.Serie.Genre,
                Plot = ws.Serie.Plot,
                Country = ws.Serie.Country,
                Poster = ws.Serie.Poster,
                ImdbRating = ws.Serie.ImdbRating,
                TotalSeasons = ws.Serie.TotalSeasons,
                Runtime = ws.Serie.Runtime,
                Actors = ws.Serie.Actors,
                Director = ws.Serie.Director,
                Writer = ws.Serie.Writer
            }).ToList();
        }
    }
}