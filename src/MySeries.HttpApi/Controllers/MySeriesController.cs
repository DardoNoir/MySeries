using Microsoft.AspNetCore.Mvc;
using MySeries.Application.Contracts;
using MySeries.Application.Usuarios;
using MySeries.Notifications;
using MySeries.Qualifications;
using MySeries.Series;
using MySeries.SerieService;
using MySeries.Usuarios;
using MySeries.Watchlists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers
{
    [Route("api/app/serie")]
    public class SerieController : AbpController
    {
        private readonly SerieAppService _serieAppService;

        public SerieController(SerieAppService serieAppService)
        {
            _serieAppService = serieAppService;
        }

        [HttpGet("search-by-title")]
        public async Task<ICollection<SerieDto>> SearchByTitleAsync([FromQuery] string title, [FromQuery] string? genre)
        {
            return await _serieAppService.SearchByTitleAsync(title, genre);
        }

        [HttpGet("search-in-db-by-title")]
        public async Task<List<SerieDto>> SearchInDbByTitleAsync([FromQuery] string title)
        {
            return await _serieAppService.SearchInDbByTitleAsync(title);
        }

    }

    [Route("api/app/usuarios")]
    public class UsuariosController : AbpController
    {
        private readonly UsuariosAppService _usuariosAppService;

        public UsuariosController(UsuariosAppService usuariosAppService)
        {
            _usuariosAppService = usuariosAppService;
        }

        [HttpGet("crear-usuario")]
        public async Task<UsuarioDto> CrearUsuarioAsync(
            [FromQuery] string userName,
            [FromQuery] string password,
            [FromQuery] string? email,
            [FromQuery] bool notificationsByEmail = false,
            [FromQuery] bool notificationsByApp = false
        )
        {
            var input = new CreateUsuarioDto
            {
                UserName = userName,
                Password = password,
                Email = email,
                NotificationsByEmail = notificationsByEmail,
                NotificationsByApp = notificationsByApp
            };

            return await _usuariosAppService.CrearUsuarioAsync(input);
        }
    }


    [Route("api/app/notifications")]
    public class NotificationsController : AbpController
    {
        private readonly NotificationsAppService _notificationsAppService;

        public NotificationsController(NotificationsAppService notificationsAppService)
        {
                _notificationsAppService = notificationsAppService;
        }

        [HttpGet("send-notification")]
        public async Task SendNotificationAsync([FromQuery] int userId, [FromQuery] string message)
        {
            await _notificationsAppService.SendNotificationAsync(userId, message);
        }

        [HttpGet("notify-by-email")]
        public async Task NotifyByEmailAsync([FromQuery] int userId, [FromQuery] string message)
        {
            await _notificationsAppService.NotifyByEmailAsync(userId, message);
        }

        [HttpGet("mark-readen")]
        public async Task MarkReadenAsync([FromQuery] int notificationId)
        {
            await _notificationsAppService.MarkReadenAsync(notificationId);
        }
    }

    [Route("api/app/qualifications")]
    public class QualificationsController : AbpController
    {
        private readonly QualificationsAppService _qualificationsAppService;
        public QualificationsController(QualificationsAppService qualificationsAppService)
        {
            _qualificationsAppService = qualificationsAppService;
        }

        [HttpGet("qualifications-series")]
        public async Task QualificationsSeriesAsync(
            [FromQuery] int userId,
            [FromQuery] int serieId,
            [FromQuery] int score,
            [FromQuery] string? review)
        {
            await _qualificationsAppService.QualificationsSeriesAsync(
                userId,
                serieId,
                score,
                review
            );
        }


        [HttpGet("modify-qualification")]
        public async Task ModifyQualificationAsync(
            [FromQuery] int userId,
            [FromQuery] int serieId,
            [FromQuery] int newScore,
            [FromQuery] string? newReview)
        {
            await _qualificationsAppService.ModifyQualificationAsync(
                userId,
                serieId,
                newScore,
                newReview
            );
        }
    }


    [Route("api/app/watchlists")]
    public class WatchlistsController : AbpController
    {
        private readonly WatchlistsAppService _watchlistsAppService;

        public WatchlistsController(WatchlistsAppService watchlistsAppService)
        {
            _watchlistsAppService = watchlistsAppService;
        }

        [HttpGet("series-from-api")]
        public async Task AddSeriesFromApiAsync([FromQuery] string imdbId, [FromQuery] int userId)
        {
            await _watchlistsAppService.AddSeriesFromApiAsync(imdbId, userId);
        }

        [HttpGet("get-watchlist")]
        public async Task<ICollection<SerieDto>> GetWatchlistAsync([FromQuery] int userId)
        {
            return await _watchlistsAppService.GetWatchlistAsync(userId);
        }

        // ✅ Eliminar serie de watchlist
        [HttpGet("remove-series")]
        public async Task RemoveSeriesAsync([FromQuery] int serieId, [FromQuery] int userId)
        {
            await _watchlistsAppService.RemoveSeriesAsync(serieId, userId);
        }
    }
}