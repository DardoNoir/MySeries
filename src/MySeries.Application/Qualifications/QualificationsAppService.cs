using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Serie, int> _seriesRepository;
        public QualificationsAppService(
            IRepository<Qualification, int> qualificationsRepository,
            IRepository<WatchList, int> watchlistsRepository,
            IRepository<Serie, int> seriesRepository)
        {
            _qualificationsRepository = qualificationsRepository;
            _watchlistsRepository = watchlistsRepository;
            _seriesRepository = seriesRepository;
        }

        // Calificar una serie


        public async Task QualificationsSeriesAsync(int userId, int serieId, int Score, string? Review)
        {
            // Verificar que esté autenticado
            if (userId <= 0)
                throw new BusinessException("Usuario no autenticado.");

            // Validar que la puntuación esté entre 1 y 10
            if (Score < 1 || Score > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            // Verificar que la serie exista
            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            // Verificar que la serie esté en la lista de seguimiento del usuario
            var watchlist = await (await _watchlistsRepository
                .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w =>
                w.UserId == userId &&
                w.SeriesList.Any(s => s.Id == serieId));

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
                var qualification = new Qualification(userId, serieId, Score, Review);
                await _qualificationsRepository.InsertAsync(qualification);
            }
        }

        // Modificar una calificación existente
        public async Task ModifyQualificationAsync(int userId, int serieId, int NewScore, string? NewReview)
        {
            // Verificar que esté autenticado
            if (userId <= 0)
                throw new BusinessException("Usuario no autenticado.");

            // Validar que la nueva puntuación esté entre 1 y 10
            if (NewScore < 1 || NewScore > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            // Verificar que la serie exista
            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            // Verificar que la serie esté en la lista de seguimiento del usuario
            var watchlist = await (await _watchlistsRepository
                .WithDetailsAsync(w => w.SeriesList))
                .FirstOrDefaultAsync(w =>
                    w.UserId == userId &&
                    w.SeriesList.Any(s => s.Id == serieId));

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