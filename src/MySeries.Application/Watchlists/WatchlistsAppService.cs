using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using MySeries.Application.Contracts;
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
    // Servicio de aplicación encargado de la lógica de las watchlists
    public class WatchlistsAppService : ApplicationService, IWatchlistsAppService
    {
        // Repositorio de WatchList
        private readonly IRepository<WatchList, int> _watchlistRepository;
        // Repositorio de Series
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly ISeriesAppService _serieAppService;


        // Constructor con inyección de dependencias
        public WatchlistsAppService(
            IRepository<WatchList, int> watchlistRepository,
            IRepository<Serie, int> serieRepository,
            ISeriesAppService serieAppService
        )
        {
            _watchlistRepository = watchlistRepository;
            _serieRepository = serieRepository;
            _serieAppService = serieAppService;
        }

        // Agrega una serie a la watchlist del usuario
        public async Task AddSeriesFromApiAsync(string imdbId, int userId)
        {
            if (userId <= 0)
                throw new BusinessException("UsuarioNoAutenticado");

            var serie = await _serieAppService.GetOrCreateFromApiAsync(imdbId);

            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
            {
                watchlist = new WatchList(userId);
                watchlist.SeriesList.Add(
                    await _serieRepository.GetAsync(serie.Id));
                await _watchlistRepository.InsertAsync(watchlist);
                return;
            }

            if (!watchlist.SeriesList.Any(s => s.ImdbId == imdbId))
            {
                var entity = await _serieRepository.GetAsync(serie.Id);
                watchlist.SeriesList.Add(entity);
                await _watchlistRepository.UpdateAsync(watchlist);
            }
        }


        // Elimina una serie de la watchlist del usuario
        public async Task RemoveSeriesAsync(int seriesId, int userId)
        {
            // Verificar autenticación
            if (userId <= 0)
                throw new BusinessException("Usuario no Autenticado.");

            // Obtener la watchlist con las series cargadas
            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
                throw new BusinessException("Lista de seguimiento no encontrada.");

            // Buscar la serie dentro de la watchlist
            var serie = watchlist.SeriesList.FirstOrDefault(s => s.Id == seriesId);
            if (serie == null)
                throw new BusinessException("Serie no encontrada en la lista de seguimiento.");

            // Eliminar la serie y actualizar
            watchlist.SeriesList.Remove(serie);
            await _watchlistRepository.UpdateAsync(watchlist);
        }

        // Obtiene la watchlist del usuario
        public async Task<ICollection<SerieDto>> GetWatchlistAsync(int userId)
        {
            // Validar autenticación
            if (userId <= 0)
                throw new BusinessException("Usuario no Autenticado.");

            // Obtener la watchlist con series
            var watchlist = await (await _watchlistRepository
                .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            // Si no hay series, devolver lista vacía
            if (watchlist == null || watchlist.SeriesList == null || !watchlist.SeriesList.Any())
                return new List<SerieDto>();

            // Validar que solo exista una watchlist por usuario
            var count = await _watchlistRepository.CountAsync(w => w.UserId == userId);
            if (count > 1)
                throw new BusinessException("Inconsistencia: múltiples watchlists para el mismo usuario.");

            // Mapear las series a DTO
            return watchlist.SeriesList.Select(s => new SerieDto
            {
                Title = s.Title,
                Year = s.Year,
                Poster = s.Poster,
                ImdbId = s.ImdbId,
            }).ToList();
        }
    }
}
