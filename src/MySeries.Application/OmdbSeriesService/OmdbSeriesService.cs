using Microsoft.Extensions.Options;
using MySeries.Application.Contracts.OmdbService;
using MySeries.SerieService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MySeries.Application
{
    public class OmdbSeriesService : IOmdbSeriesService
    {
        private readonly HttpClient _httpClient;
        private readonly OmdbOptions _options;

        public OmdbSeriesService(HttpClient httpClient, IOptions<OmdbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<ICollection<SerieDto>> SearchByTitleAsync(string title)
        {
            var url = $"?apikey={_options.ApiKey}&type=series&s={title}";
            var result = await _httpClient.GetFromJsonAsync<ICollection<SerieDto>>(url);

            if (result == null)
                throw new InvalidOperationException($"No se pudo obtener resultados de OMDb para title={title}");

            return result;
        }

        public async Task<SerieDto> GetByImdbIdAsync(string imdbId)
        {
            var url = $"?apikey={_options.ApiKey}&i={imdbId}&plot=full";
            var result = await _httpClient.GetFromJsonAsync<SerieDto>(url);

            if (result == null)
                throw new InvalidOperationException($"No se pudo obtener detalles de OMDb para imdbId={imdbId}");

            return result;
        }
    }
}
