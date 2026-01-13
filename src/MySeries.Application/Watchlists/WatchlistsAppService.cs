using JetBrains.Annotations;
using Microsoft.Extensions.Configuration.UserSecrets;
using MySeries.Series;
using MySeries.SerieService;
using MySeries.WatchLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace MySeries.Watchlists
{
    public class WatchlistsAppService: ApplicationService, IWatchlistsAppService
    {
        private readonly IRepository<WatchList, int> _watchlistRepository;
        private readonly IRepository<Serie, Guid> _serieRepository;
        private readonly ICurrentUser _currentUser;

        public WatchlistsAppService(IRepository<WatchList, int> watchlistRepository,
            IRepository<Serie, Guid> serieRepository,
            ICurrentUser currentUser)
        {
            _watchlistRepository = watchlistRepository;
            _serieRepository = serieRepository;
            _currentUser = currentUser;
        }

        //Agregar una serie a la watchlist del usuario actual
        public async Task AddSeriesAsync(Guid seriesId)
        {
            // Obtener el ID del usuario actual Y verificar si está autenticado
            Guid? userId = _currentUser.Id;
            if (!userId.HasValue)
                throw new BusinessException("Usuario no Autenticado.");
            
            // Verificar si la serie existe
            var serie = await _serieRepository.GetAsync(seriesId);
            if (serie == null)
                throw new BusinessException("Serie no encontrada.");

            // Obtener o crear la watchlist del usuario
            var watchlist = await _watchlistRepository.FirstOrDefaultAsync(w => w.UserId == userId.Value);
            if (watchlist == null)
            {
                watchlist = new WatchList(userId.Value);
                await _watchlistRepository.InsertAsync(watchlist);
            }

            // Agregar la serie a la watchlist si no está ya presente
            if (!watchlist.SeriesList.Any(s => s.Id == seriesId))
            {
                watchlist.SeriesList.Add(serie);
                await _watchlistRepository.UpdateAsync(watchlist);
            }
        }

        public async Task RemoveSeriesAsync(Guid seriesId)
        {
            // Obtener el ID del usuario actual y verificar si está autenticado
            Guid? userId = _currentUser.Id;
            if (!userId.HasValue)
                throw new BusinessException("Usuario no Autenticado.");

            // Obtener la watchlist del usuario
            var watchlist = await _watchlistRepository.FirstOrDefaultAsync(w => w.UserId == userId.Value);
            if (watchlist == null)
                throw new BusinessException("Lista de seguimiento no encontrada.");

            // Buscar la serie en la watchlist
            var serie = watchlist.SeriesList.FirstOrDefault(s => s.Id == seriesId);
            if (serie == null)
                throw new BusinessException("Serie no encontrada en la lista de seguimiento.");

            // Eliminar la serie de la watchlist
            watchlist.SeriesList.Remove(serie);

            // Actualizar la watchlist en el repositorio
            await _watchlistRepository.UpdateAsync(watchlist);
        }

        public async Task<ICollection<SerieDto>> GetWatchlistAsync()
        {
            // Obtener el ID del usuario actual y verificar si está autenticado
            Guid? userId = _currentUser.Id;
            if (!userId.HasValue)
                throw new BusinessException("Usuario no Autenticado.");

            // Obtener la watchlist del usuario
            var watchlist = await _watchlistRepository.FirstOrDefaultAsync(w => w.UserId == userId.Value);
            if (watchlist == null || !watchlist.SeriesList.Any())
                throw new BusinessException("Lista de seguimiento vacía o no encontrada.");

            // Verificar que solo exista una watchlist por usuario
            var count = await _watchlistRepository.CountAsync(w => w.UserId == userId.Value);
            if (count > 1)
                throw new BusinessException("Inconsistencia: múltiples watchlists para el mismo usuario.");

            // Mapear las series a SerieDto
            ICollection<SerieDto> series = watchlist.SeriesList.
                Select(s => new SerieDto
            {
                ImdbId = s.ImdbId,
                Title = s.Title,
                Year = s.Year,
                Genre = s.Genre,
                Plot = s.Plot,
                Country = s.Country,
                Poster = s.Poster,
                ImdbRating = s.ImdbRating,
                TotalSeasons = s.TotalSeasons,
                Runtime = s.Runtime,
                Actors = s.Actors,
                Director = s.Director,
                Writer = s.Writer
            }).ToList();

            // Retornar la colección de SerieDto
            return series;

        }
    }
}
