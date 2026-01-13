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

        public async Task QualificationsSeriesAsync(Guid serieId, int Score, string? Review = null)
        {
            Guid? userId = _currentUser.Id;
            if (!_currentUser.Id.HasValue)
                throw new BusinessException("Usuario no autenticado.");

            if (Score < 1 || Score > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            var watchlist = await _watchlistsRepository.
                FirstOrDefaultAsync(w => w.UserId == userId && w.SeriesList.Contains(serie));
            if (watchlist == null)
                throw new BusinessException("La serie no está en la lista de seguimiento del usuario.");

            var qualificated = await _qualificationsRepository.
                FirstOrDefaultAsync(q => q.UserId == userId 
                && q.SerieId == serieId);

            if (qualificated != null)
            {
                qualificated.Score = Score;
                qualificated.Review = Review;
                await _qualificationsRepository.UpdateAsync(qualificated);
            }
            else
            {
                var qualification = new Qualification(userId.Value, serieId, Score, Review);

                await _qualificationsRepository.InsertAsync(qualification);
            }
        }

        public async Task ModifyQualificationAsync(Guid serieId, int NewScore, string? NewReview = null)
        {
            Guid? userId = _currentUser.Id;

            if (!userId.HasValue)
                throw new BusinessException("Usuario no autenticado.");

            if (NewScore < 1 || NewScore > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");

            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");

            var watchlist = await _watchlistsRepository.
                FirstOrDefaultAsync(w => w.UserId == userId && w.SeriesList.Contains(serie));
            if (watchlist == null)
                throw new BusinessException("La serie no está en la lista de seguimiento del usuario.");

            var qualificated = await _qualificationsRepository.
                FirstOrDefaultAsync(q => q.UserId == userId && q.SerieId == serieId);

            if (qualificated == null)
                throw new BusinessException("No existe una calificación previa para esta serie.");

            qualificated.Score = NewScore;
            qualificated.Review = NewReview;

            await _qualificationsRepository.UpdateAsync(qualificated);
        }
    }
}
