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
        private static readonly string apiKey = "844b1b8b"; // Reemplaza con tu clave API de OMDb.
        private static readonly string baseUrl = "http://www.omdbapi.com/";

        public async Task<ICollection<SerieDto>> GetSeriesAsync(string title)
        {
            using var client = new HttpClient();

            var url = $"{baseUrl}?s={title}&type=series&apikey={apiKey}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var omdbResponse = JsonConvert.DeserializeObject<OmdbSearchResponse>(json);

            var result = new List<SerieDto>();

            foreach (var item in omdbResponse?.Search ?? [])
            {
                result.Add(new SerieDto
                {
                    Title = item.Title,
                    Year = item.Year,
                    Poster = item.Poster,
                });
            }

            return result;
        }
    }
}

