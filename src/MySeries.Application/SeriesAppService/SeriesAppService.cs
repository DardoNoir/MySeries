using MySeries.Application.Contracts;
using MySeries.Application.Contracts.OmdbService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Application.Series
{
    public class SeriesAppService : ApplicationService, ISeriesAppService
    {
        private readonly IOmdbSeriesService _omdbSeriesService;

        public SeriesAppService(IOmdbSeriesService omdbSeriesService)
        {
            _omdbSeriesService = omdbSeriesService;
        }

        // Obtener detalle completo de una serie
        public async Task<OmdbSeriesDto> GetFromOmdbAsync(string imdbId)
        {
            return await _omdbSeriesService.GetByImdbIdAsync(imdbId);
        }

 
        // Buscar series por título y opcionalmente filtrar por género.
        // Devuelve OmdbSeriesSearchDto enriquecido con el género.
 
        public async Task<OmdbSeriesSearchDto> SearchFromOmdbAsync(string title, string? genre = null)
        {
            var searchResult = await _omdbSeriesService.SearchByTitleAsync(title);

            if (searchResult?.Search == null)
                return searchResult ?? new OmdbSeriesSearchDto();

            var filtered = new List<OmdbSeriesSearchItemDto>();

            foreach (var item in searchResult.Search)
            {
                var details = await _omdbSeriesService.GetByImdbIdAsync(item.ImdbId);

                item.Genre = details.Genre; // enriquecemos el SearchItem

                if (string.IsNullOrWhiteSpace(genre) ||
                    (item.Genre != null && item.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)))
                {
                    filtered.Add(item);
                }
            }

            searchResult.Search = filtered;
            searchResult.TotalResults = filtered.Count.ToString();

            return searchResult;
        }
    }
}



