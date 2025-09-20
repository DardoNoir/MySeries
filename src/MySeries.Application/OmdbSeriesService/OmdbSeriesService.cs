
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options; 
using MySeries.Application.Contracts.OmdbService;

namespace MySeries.Application
{
    public class OmdbSeriesService : IOmdbSeriesService
    {
        private readonly HttpClient _httpClient;
        private readonly OmdbOptions _options;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public OmdbSeriesService(HttpClient httpClient, IOptions<OmdbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        public async Task<OmdbSeriesDto> GetByImdbIdAsync(string imdbId)
        {
            var url = $"?i={Uri.EscapeDataString(imdbId)}&type=series&apikey={_options.ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<OmdbSeriesDto>(json, _jsonOptions);
            if (dto == null)
            {
                throw new InvalidOperationException($"No se pudo deserializar la respuesta de OMDb para imdbId={imdbId}.");
            }
            return dto;
        }

        public async Task<OmdbSeriesSearchDto> SearchByTitleAsync(string title)
        {
            var url = $"?s={Uri.EscapeDataString(title)}&type=series&apikey={_options.ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<OmdbSeriesSearchDto>(json, _jsonOptions);
            if (dto == null)
            {
                throw new InvalidOperationException($"No se pudo deserializar la respuesta de OMDb para title={title}.");
            }
            return dto;
        }
    }
}