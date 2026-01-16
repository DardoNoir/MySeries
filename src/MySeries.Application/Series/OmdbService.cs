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
        private static readonly string apiKey = "fa5ffac0"; // Reemplaza con tu clave API de OMDb.
        private static readonly string baseUrl = "http://www.omdbapi.com/";

        public async Task<ICollection<serieDto>> GetSeriesAsync(string title)
        {
            using HttpClient client = new HttpClient();

            List<serieDto> series = new List<serieDto>();

            string url = $"{baseUrl}?s={title}&apikey={apiKey}&type=series";

            try
            {
                // Hacer la solicitud HTTP y obtener la respuesta como string
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta JSON a un objeto SearchResponse
                var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(jsonResponse);

                // Retornar la lista de series si existen
                var seriesOmdb = searchResponse?.Search ?? new List<SerieOmdb>();

                foreach (var serieOmdb in seriesOmdb)
                {
                    series.Add(new serieDto
                    {
                        Title = serieOmdb.Title.ToString(),
                        Year = serieOmdb.Year.ToString(),
                        Genre = serieOmdb.Genre.ToString()
                    });
                }
                return series;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Se ha producido un error en la búsqueda de la serie", e);
            }
        }

        private class SearchResponse
        {
            [JsonProperty("Search")]
            public List<SerieOmdb> Search { get; set; }
        }
        private class SerieOmdb
        {
            public required string Title { get; set; }
            public required string Genre { get; set; }
            public string? Plot { get; set; }
            public required string Year { get; set; }
            public string? Country { get; set; }
            public string? ImdbId { get; set; } // relation with OMDb
            public string? ImdbRating { get; set; }
            public string? TotalSeasons { get; set; } // temporadas
            public string? Poster { get; set; }
            public string? Runtime { get; set; }
            public string? Actors { get; set; }
            public string? Director { get; set; }
            public string? Writer { get; set; }

        }
    }
}

