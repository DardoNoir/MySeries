using Microsoft.Extensions.Logging;
using MySeries.Notifications;
using MySeries.Watchlists;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using System;
using System.Linq;
using System.Threading.Tasks;
using MySeries.Series;
using Microsoft.Extensions.DependencyInjection;
using MySeries.SerieService;
using System.Collections.Generic;
using MySeries.Usuarios;

public class SerieUpdateWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IRepository<Serie, int> _serieRepository;
    private readonly ISeriesApiService _seriesApiService;
    private readonly IRepository<WatchListSerie> _watchListSerieRepository;
    private readonly NotificationsAppService _notificationsAppService;
    private readonly IRepository<WatchList,int> _watchListRepository;
    private readonly IRepository<Usuario, int> _userRepository;

    public SerieUpdateWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IRepository<Serie, int> serieRepository,
        ISeriesApiService seriesApiService,
        IRepository<WatchListSerie> watchListSerieRepository,
        NotificationsAppService notificationsAppService,
        IRepository<WatchList, int> watchListRepository,
        IRepository<Usuario, int> userRepository

    ) : base(timer, serviceScopeFactory)
    {
        
        //Timer.Period = 6 * 60 *60 *1000;
        // Timer para pruebas
         Timer.Period = 30 * 1000; 
        
        _serieRepository = serieRepository;
        _seriesApiService = seriesApiService;
        _watchListSerieRepository = watchListSerieRepository;
        _notificationsAppService = notificationsAppService;
        _watchListRepository = watchListRepository;
        _userRepository = userRepository;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var series = await _serieRepository.GetListAsync();
        Logger.LogInformation($"Entro al worker");

        foreach (var serie in series)
        {
            if (string.IsNullOrWhiteSpace(serie.ImdbId))
                continue;

            var apiSerie = await _seriesApiService.GetSerieByImdbIdAsync(serie.ImdbId);
            if (apiSerie == null)
                continue;

            var changes = DetectChanges(serie, apiSerie);
            if (!changes.Any())
                continue;

            // Actualizar serie
            serie.ImdbRating = apiSerie.ImdbRating;
            serie.TotalSeasons = apiSerie.TotalSeasons;
            serie.Poster = apiSerie.Poster;
            serie.Director = apiSerie.Director;

            await _serieRepository.UpdateAsync(serie, autoSave: true);

            // Usuarios que siguen la serie
            var watchListsSerie = await _watchListSerieRepository.GetListAsync(
                ws => ws.SerieId == serie.Id
            );

            var watchlistIds = watchListsSerie.Select(ws => ws.WatchListId).ToList();

            foreach (var id in watchlistIds)
            {

                var wls = await _watchListRepository.GetAsync(id);
                if (wls == null)
                    continue;

                var user = await _userRepository.GetAsync(wls.UserId);

                if (user.NotificationsByApp)
                {
                    await _notificationsAppService.SendNotificationAsync(
                    user.Id,
                    $"📺 Cambios en \"{serie.Title}\": {string.Join(", ", changes)}");
                }

                if (user.NotificationsByEmail)
                {
                    await _notificationsAppService.NotifyByEmailAsync(user.Id,
                        $"📺 Cambios en \"{serie.Title}\": {string.Join(", ", changes)}");
                }
            }
        }
    }

    private List<string> DetectChanges(Serie serie, SerieDto apiSerie)
    {
        var changes = new List<string>();

        if (serie.ImdbRating != apiSerie.ImdbRating)
            changes.Add($"nueva valoración general de {apiSerie.Title}: {apiSerie.ImdbRating}/10");

        if (serie.TotalSeasons != apiSerie.TotalSeasons)
            changes.Add($"Nueva temporada de {apiSerie.Title}, ahora cuenta con: {apiSerie.TotalSeasons} temporadas");

        if (serie.Poster != apiSerie.Poster)
            changes.Add($"Nueva imagen en {apiSerie.Title}");

        if (serie.Director != apiSerie.Director)
            changes.Add($"Nuevo director en {apiSerie.Title}: {apiSerie.Director}");

        return changes;
    }
}
