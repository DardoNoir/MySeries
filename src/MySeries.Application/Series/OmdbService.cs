using MySeries.Application.Contracts;
using MySeries.SerieService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MySeries.Series
{
    public class OmdbService : ISeriesApiService, ITransientDependency
    {
        // API Key de OMDb
        private static readonly string apiKey = "844b1b8b";
        // URL base de la API de OMDb
        private static readonly string baseUrl = "http://www.omdbapi.com/";

        // Búsqueda de series por título y género
        public async Task<ICollection<SerieDto>> GetSeriesAsync(string title, string? genre)
        {
            using var client = new HttpClient();

            // Primero se realiza la búsqueda por Título para obtener el ImdbId
            var searchUrl = $"{baseUrl}?s={title}&type=series&apikey={apiKey}";
            var searchResponse = await client.GetAsync(searchUrl);
            searchResponse.EnsureSuccessStatusCode();

            var searchJson = await searchResponse.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<OmdbSearchResponse>(searchJson);

            var result = new List<SerieDto>();

            var genreFilter = genre?.Trim().ToLowerInvariant();

            // Obtención del resto de datos de la Serie, mediante ImdbId
            foreach (var item in searchResult?.Search ?? [])
            {
                if (string.IsNullOrWhiteSpace(item.ImdbId))
                    continue;

                var detailUrl = $"{baseUrl}?i={item.ImdbId}&apikey={apiKey}";
                var detailResponse = await client.GetAsync(detailUrl);

                if (!detailResponse.IsSuccessStatusCode)
                    continue;

                var detailJson = await detailResponse.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<OmdbDetailResponse>(detailJson);

                // Filtro por género 
                if (!string.IsNullOrEmpty(genreFilter))
                {
                    if (string.IsNullOrWhiteSpace(detail?.Genre) ||
                    !(detail.Genre).ToLowerInvariant().Contains(genreFilter))
                    {
                        continue; 
                    }
                }

                // Agregar los datos a SerieDto
                result.Add(new SerieDto
                {
                    Title = item.Title,
                    Year = item.Year,
                    Poster = item.Poster,
                    Genre = detail?.Genre,
                    Plot = detail?.Plot,
                    Country = detail?.Country,
                    ImdbId = item.ImdbId,
                    ImdbRating = detail?.ImdbRating,
                    TotalSeasons = detail?.TotalSeasons,
                    Runtime = detail?.Runtime,
                    Actors = detail?.Actors,
                    Director = detail?.Director,
                    Writer = detail?.Writer
                });
            }

            return result;
        }


        // Búsqueda de Series por ImdbId, usado para agregar Series a la Watchlist
        public async Task<SerieDto?> GetSerieByImdbIdAsync(string imdbId)
        {
            using var client = new HttpClient();

            var url = $"{baseUrl}?i={imdbId}&type=series&apikey={apiKey}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var detail = JsonConvert.DeserializeObject<OmdbDetailResponse>(json);

            if (detail == null)
                return null;

            return new SerieDto
            {
                ImdbId = imdbId,
                Title = detail.Title!,
                Genre = detail.Genre,
                Year = detail.Year!,
                Plot = detail.Plot,
                Country = detail.Country,
                ImdbRating = detail.ImdbRating,
                TotalSeasons = detail.TotalSeasons,
                Runtime = detail.Runtime,
                Actors = detail.Actors,
                Director = detail.Director,
                Writer = detail.Writer,
                Poster = detail.Poster
            };
        }
    }
}
