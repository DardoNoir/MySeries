using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Serie, int> _serieRepository;

        public WatchlistsAppService(IRepository<WatchList, int> watchlistRepository,
            IRepository<Serie, int> serieRepository,
            ICurrentUser currentUser)
        {
            _watchlistRepository = watchlistRepository;
            _serieRepository = serieRepository;
        }

        //Agregar una serie a la watchlist del usuario actual
        public async Task AddSeriesAsync(int seriesId, int userId)
        {
            if (userId <= 0)
                throw new BusinessException("Usuario no Autenticado.");

            var serie = await _serieRepository.GetAsync(seriesId);
            if (serie == null)
                throw new BusinessException("Serie no encontrada.");

            var watchlist = await (await _watchlistRepository
                    .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
            {
                watchlist = new WatchList(userId)
                {
                    SeriesList = new List<Serie>()
                };

                watchlist.SeriesList.Add(serie);
                await _watchlistRepository.InsertAsync(watchlist);
                return;
            }

            watchlist.SeriesList ??= new List<Serie>();

            if (!watchlist.SeriesList.Any(s => s.Id == seriesId))
            {
                watchlist.SeriesList.Add(serie);
                await _watchlistRepository.UpdateAsync(watchlist);
            }
        }


        public async Task RemoveSeriesAsync(int seriesId, int userId)
        {
            if (userId <= 0)
            {
                throw new BusinessException("Usuario no Autenticado.");
            }

            // 2. Obtener la watchlist INCLUYENDO la colección de series
            // Usamos WithDetailsAsync para cargar SeriesList de forma ansiosa (Eager Loading)
            var watchlist = await (await _watchlistRepository.WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
            {
                throw new BusinessException("Lista de seguimiento no encontrada.");
            }

            // 3. Buscar la serie dentro de la lista cargada
            var serie = watchlist.SeriesList.FirstOrDefault(s => s.Id == seriesId);

            if (serie == null)
            {
                throw new BusinessException("Serie no encontrada en la lista de seguimiento.");
            }

            // 4. Eliminar la serie de la colección y actualizar
            watchlist.SeriesList.Remove(serie);

            // ABP se encargará de actualizar la relación en la base de datos
            await _watchlistRepository.UpdateAsync(watchlist);
        }

        public async Task<ICollection<SerieDto>> GetWatchlistAsync(int userId)
        {
            // Verificar si está autenticado
            if (userId <= 0)
                throw new BusinessException("Usuario no Autenticado.");

            // Obtener la watchlist del usuario
            var watchlist = await (await _watchlistRepository.WithDetailsAsync(w => w.SeriesList)).
                FirstOrDefaultAsync(w => w.UserId == userId);
            if (watchlist == null || watchlist.SeriesList == null || !watchlist.SeriesList.Any())
            {
                return new List<SerieDto>(); // Es mejor devolver una lista vacía que lanzar excepción
            }

            // Verificar que solo exista una watchlist por usuario
            var count = await _watchlistRepository.CountAsync(w => w.UserId == userId);
            if (count > 1)
                throw new BusinessException("Inconsistencia: múltiples watchlists para el mismo usuario.");

            // Mapear las series a SerieDto
            return watchlist.SeriesList.Select(s => new SerieDto
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

        }
    }
}
