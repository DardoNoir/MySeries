using Microsoft.EntityFrameworkCore;
using MySeries.Notifications;
using MySeries.Series;
using MySeries.SerieService;
using MySeries.Usuarios;
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
        private readonly IRepository<Qualification, int> _qualificationRepository;
        private readonly NotificationsAppService _notificationsAppService;
        private readonly IRepository<Usuario, int> _userRepository;


        public WatchlistsAppService(
            IRepository<WatchList, int> watchlistRepository,
            IRepository<Serie, int> serieRepository,
            IRepository<WatchListSerie> watchListSerieRepository,
            SerieAppService serieAppService,
            IRepository<Qualification, int> qualificationRepository,
            NotificationsAppService notificationsAppService,
            IRepository<Usuario,int> userRepositry)
        {
            _watchlistRepository = watchlistRepository;
            _serieRepository = serieRepository;
            _watchListSerieRepository = watchListSerieRepository;
            _serieAppService = serieAppService;
            _qualificationRepository = qualificationRepository;
            _notificationsAppService = notificationsAppService;
            _userRepository = userRepositry;
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
            var user = await _userRepository.GetAsync(userId);

            // 4️⃣ Obtener la watchlist del usuario incluyendo sus series
            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(w => w.WatchListSeries))
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

             if (user.NotificationsByApp)
            {
                await _notificationsAppService.SendNotificationAsync(
                    userId,
                    $"⭐ Agregaste \"{serieEntity.Title}\" a tu lista de seguimiento"
                );
            }

            if (user.NotificationsByEmail)
            {
                await _notificationsAppService.NotifyByEmailAsync(
                    userId,
                    $"⭐ Agregaste \"{serieEntity.Title}\" a tu lista de seguimiento"
                );
            }
        }

        // Eliminar una serie de la watchlist del usuario
        [RemoteService(IsEnabled = false)]
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

            var serie = await _serieRepository.GetAsync(seriesId);
            var user = await _userRepository.GetAsync(userId);

            // 3️⃣ Buscar la relación WatchListSerie
            var relation = await _watchListSerieRepository.FirstOrDefaultAsync(
                x => x.WatchListId == watchlist.Id && x.SerieId == seriesId);

            if (relation == null)
                throw new BusinessException("SerieNoEncontradaEnLaWatchlist");

            // 4️⃣ Eliminar la relación (NO la serie)
            await _watchListSerieRepository.DeleteAsync(relation);

            if (user.NotificationsByApp)
            {
                await _notificationsAppService.SendNotificationAsync(
                    userId,
                    $"❌ Quitaste \"{serie.Title}\" de tu lista de seguimiento"
                );
            }

            if (user.NotificationsByEmail)
            {
                await _notificationsAppService.NotifyByEmailAsync(
                    userId,
                    $"❌ Quitaste \"{serie.Title}\" de tu lista de seguimiento"
                );
            }
        }

        // Obtener la watchlist del usuario
        [RemoteService(IsEnabled = false)]
        public async Task<ICollection<WatchlistSerieDto>> GetWatchlistAsync(int userId)
{
    if (userId <= 0)
        throw new BusinessException("Usuario No Autenticado");

    var queryable = await _watchlistRepository.GetQueryableAsync();

    var watchlist = await queryable
        .Include(w => w.WatchListSeries)
            .ThenInclude(ws => ws.Serie)
        .FirstOrDefaultAsync(w => w.UserId == userId);

    if (watchlist == null || !watchlist.WatchListSeries.Any())
        return new List<WatchlistSerieDto>();

    var serieIds = watchlist.WatchListSeries
        .Where(ws => ws.Serie != null)
        .Select(ws => ws.SerieId)
        .ToList();

    var qualifications = await (await _qualificationRepository.GetQueryableAsync())
        .Where(q => q.UserId == userId && serieIds.Contains(q.SerieId))
        .ToListAsync();

    return watchlist.WatchListSeries
        .Where(ws => ws.Serie != null)
        .Select(ws =>
        {
            var qualification = qualifications
                .FirstOrDefault(q => q.SerieId == ws.SerieId);

            return new WatchlistSerieDto
            {
                Id = ws.Serie.Id,
                Title = ws.Serie.Title,
                Year = ws.Serie.Year,
                Genre = ws.Serie.Genre,
                Poster = ws.Serie.Poster,
                ImdbId = ws.Serie.ImdbId!,
                Score = qualification?.Score,
                Review = qualification?.Review
            };
        })
        .ToList();
}


    }
}