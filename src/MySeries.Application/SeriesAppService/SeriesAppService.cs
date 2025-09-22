using MySeries.Application.Contracts;
using Volo.Abp.Application.Services;
using System.Threading.Tasks;
using MySeries.Application.Contracts.OmdbService;
using System;
using System.Linq;


public class SeriesAppService : ApplicationService, ISeriesAppService
{
    private readonly IOmdbSeriesService _omdbSeriesService;

    public SeriesAppService(IOmdbSeriesService omdbSeriesService)
    {
        _omdbSeriesService = omdbSeriesService;
    }

    public async Task<OmdbSeriesDto> GetFromOmdbAsync(string imdbId)
    {
        var series = await _omdbSeriesService.GetByImdbIdAsync(imdbId);

        return new OmdbSeriesDto
        {
            Title = series.Title,
            Year = series.Year,
            Genre = series.Genre,
            Country = series.Country,
            Poster = series.Poster,
            Plot = series.Plot,
            ImdbRating = series.ImdbRating,
            TotalSeasons = series.TotalSeasons
        };
    }
   public async Task<OmdbSeriesSearchDto> SearchFromOmdbAsync(string title, string? genre = null)
    {
        var searchResult = await _omdbSeriesService.SearchByTitleAsync(title);
        if (!string.IsNullOrWhiteSpace(genre))
        {
            searchResult.Search = searchResult.Search
                .FindAll(s => s.Title.Contains(genre, StringComparison.OrdinalIgnoreCase));
        }
        return searchResult;
    }
}


