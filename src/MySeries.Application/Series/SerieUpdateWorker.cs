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

public class SerieUpdateWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IRepository<Serie, int> _serieRepository;
    private readonly ISeriesApiService _seriesApiService;
    private readonly IRepository<WatchListSerie> _watchListSerieRepository;
    private readonly NotificationsAppService _notificationsAppService;

    public SerieUpdateWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IRepository<Serie, int> serieRepository,
        ISeriesApiService seriesApiService,
        IRepository<WatchListSerie> watchListSerieRepository,
        NotificationsAppService notificationsAppService
    ) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 10 * 1000; // ⏱ cada 6 horas
        _serieRepository = serieRepository;
        _seriesApiService = seriesApiService;
        _watchListSerieRepository = watchListSerieRepository;
        _notificationsAppService = notificationsAppService;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var series = await _serieRepository.GetListAsync();
        Console.WriteLine($"Entro al worker");

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
            serie.Plot = apiSerie.Plot;

            await _serieRepository.UpdateAsync(serie, autoSave: true);

            // Usuarios que siguen la serie
            var followers = await _watchListSerieRepository.GetListAsync(
                ws => ws.SerieId == serie.Id
            );

            foreach (var follower in followers)
            {
                await _notificationsAppService.SendNotificationAsync(
                    follower.WatchList.UserId,
                    $"📺 Cambios en \"{serie.Title}\": {string.Join(", ", changes)}"
                );
                Console.WriteLine($"Se lanzó la notificación");
            }
        }
    }

    private List<string> DetectChanges(Serie serie, SerieDto apiSerie)
    {
        var changes = new List<string>();

        if (serie.ImdbRating != apiSerie.ImdbRating)
            changes.Add($"nuevo rating IMDb: {apiSerie.ImdbRating}");

        if (serie.TotalSeasons != apiSerie.TotalSeasons)
            changes.Add($"nueva cantidad de temporadas: {apiSerie.TotalSeasons}");

        if (serie.Plot != apiSerie.Plot)
            changes.Add("actualización en la descripción");

        return changes;
    }
}
