using Microsoft.EntityFrameworkCore;
using MySeries.Notifications;
using MySeries.Series;
using MySeries.Usuarios;
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
        private readonly NotificationsAppService _notificationsAppService;
        private readonly IRepository<Usuario, int> _userRepository;
        public QualificationsAppService(
            IRepository<Qualification, int> qualificationsRepository,
            IRepository<WatchList, int> watchlistsRepository,
            IRepository<Serie, int> seriesRepository,
             NotificationsAppService notificationsAppService,
            IRepository<Usuario,int> userRepositry)
        {
            _qualificationsRepository = qualificationsRepository;
            _watchlistsRepository = watchlistsRepository;
            _seriesRepository = seriesRepository;
            _notificationsAppService = notificationsAppService;
            _userRepository = userRepositry;
        }

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)]
        // Calificar O modificar la calificación de una serie 
        public async Task QualificationsSeriesAsync(int userId, int serieId, int Score, string? Review)
        {
            if (userId <= 0)
                throw new BusinessException("Usuario no autenticado.");
            
            var user = await _userRepository.GetAsync(userId);

            if (Score < 1 || Score > 10)
                throw new BusinessException("La puntuación debe estar entre 1 y 10.");


            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Id == serieId);
            if (serie == null)
                throw new BusinessException("La serie no existe.");


            var watchlist = await (await _watchlistsRepository
                .WithDetailsAsync(w => w.WatchListSeries))
                .FirstOrDefaultAsync(w =>
                w.UserId == userId &&
                w.WatchListSeries.Any(s => s.SerieId == serieId));

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

                if (user.NotificationsByApp)
                {
                    await _notificationsAppService.SendNotificationAsync(
                        userId,
                        $"🔄 Actualizaste tu calificación de \"{serie.Title}\" a {Score}/10"
                    );
                }

                if (user.NotificationsByEmail)
                {
                    await _notificationsAppService.NotifyByEmailAsync(
                        userId,
                        $"🔄 Actualizaste tu calificación de \"{serie.Title}\" a {Score}/10"
                    );
                }

            }
            else
            {
                var qualification = new Qualification(userId, serieId, Score, Review);
                await _qualificationsRepository.InsertAsync(qualification); 

                if (user.NotificationsByApp)
                {
                    await _notificationsAppService.SendNotificationAsync(
                        userId,
                        $"⭐ Calificaste la serie \"{serie.Title}\" con {Score}/10"
                    );
                }

                if (user.NotificationsByEmail)
                {
                    await _notificationsAppService.NotifyByEmailAsync(
                        userId,
                        $"⭐ Calificaste la serie \"{serie.Title}\" con {Score}/10"
                    );
                }               
            }
        }
    }
}