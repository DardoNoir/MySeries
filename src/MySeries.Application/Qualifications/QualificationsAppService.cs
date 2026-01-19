using MySeries.Series;
using MySeries.Watchlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace MySeries.Qualifications
{
    public class QualificationsAppService : ApplicationService, IQualificationsAppService
    {
        private readonly IRepository<Qualification, int> _qualificationsRepository;
        private readonly IRepository<WatchList, int> _watchlistsRepository;
        private readonly IRepository<Serie, Guid> _seriesRepository;
        private readonly ICurrentUser _currentUser;

        public QualificationsAppService(
            IRepository<Qualification, int> qualificationsRepository,
            IRepository<WatchList, int> watchlistsRepository,
            IRepository<Serie, Guid> seriesRepository,
            ICurrentUser currentUser)
        {
            _qualificationsRepository = qualificationsRepository;
            _watchlistsRepository = watchlistsRepository;
            _seriesRepository = seriesRepository;
            _currentUser = currentUser;
        }

        // Calificar una serie
        public async Task QualificationsSeriesAsync(Guid serieId, int Score, string? Review = null)
        {
            // Obtener el Id del usuario actual y verificar que esté autenticado
            Guid? userId = _currentUser.Id;
            if (!_currentUser.Id.HasValue)
                throw new BusinessException("Usuario no autenticado.");

            // Validar que la puntuación esté entre 1 y 10
            if (Score < 1 || Score > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            // Verificar que la serie exista
            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            // Verificar que la serie esté en la lista de seguimiento del usuario
            var watchlist = await _watchlistsRepository.
                FirstOrDefaultAsync(w => w.UserId == userId && w.SeriesList.Contains(serie));
            if (watchlist == null)
                throw new BusinessException("La serie no está en la lista de seguimiento del usuario.");

            // Verificar si el usuario ya ha calificado la serie
            var qualificated = await _qualificationsRepository.
                FirstOrDefaultAsync(q => q.UserId == userId 
                && q.SerieId == serieId);

            // Si ya la ha calificado, actualizar la calificación y reseña
            if (qualificated != null)
            {
                qualificated.Score = Score;
                qualificated.Review = Review;
                await _qualificationsRepository.UpdateAsync(qualificated);
            }
            // Si no la ha calificado, crear una nueva calificación
            else
            {
                var qualification = new Qualification(userId.Value, serieId, Score, Review);
                await _qualificationsRepository.InsertAsync(qualification);
            }
        }

        // Modificar una calificación existente
        public async Task ModifyQualificationAsync(Guid serieId, int NewScore, string? NewReview = null)
        {
            // Obtener el Id del usuario actual y verificar que esté autenticado
            Guid? userId = _currentUser.Id;
            if (!userId.HasValue)
                throw new BusinessException("Usuario no autenticado.");

            // Validar que la nueva puntuación esté entre 1 y 10
            if (NewScore < 1 || NewScore > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            // Verificar que la serie exista
            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            // Verificar que la serie esté en la lista de seguimiento del usuario
            var watchlist = await _watchlistsRepository.
                FirstOrDefaultAsync(w => w.UserId == userId && w.SeriesList.Contains(serie));
            if (watchlist == null)
                throw new BusinessException("La serie no está en la lista de seguimiento del usuario.");

            // Verificar si el usuario ha calificado la serie previamente
            var qualificated = await _qualificationsRepository.
                FirstOrDefaultAsync(q => q.UserId == userId && q.SerieId == serieId);

            // Si no ha calificado la serie, lanzar una excepción
            if (qualificated == null)
                throw new BusinessException("No existe una calificación previa para esta serie.");

            // Actualizar la calificación y reseña
            qualificated.Score = NewScore;
            qualificated.Review = NewReview;
            await _qualificationsRepository.UpdateAsync(qualificated);
        }
    }
}
