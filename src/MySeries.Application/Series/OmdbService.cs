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
    // Servicio que se encarga de consumir la API externa OMDb
    // Implementa ISeriesApiService para desacoplar la lógica del proveedor
    // ITransientDependency indica que se crea una nueva instancia por inyección
    public class OmdbService : ISeriesApiService, ITransientDependency
    {
        // API Key de OMDb
        private static readonly string apiKey = "844b1b8b";
        // URL base de la API de OMDb
        private static readonly string baseUrl = "http://www.omdbapi.com/";

        // Obtiene una colección de series filtradas por título
        public async Task<ICollection<SerieDto>> GetSeriesAsync(string title)
        {
            // Crear el cliente HTTP para realizar la petición
            using var client = new HttpClient();

            // Construir la URL con parámetros de búsqueda
            var url = $"{baseUrl}?s={title}&type=series&apikey={apiKey}";

            // Ejecutar la petición GET a la API
            var response = await client.GetAsync(url);
            // Lanza excepción si la respuesta no es exitosa
            response.EnsureSuccessStatusCode();

            // Leer el contenido de la respuesta como string
            var json = await response.Content.ReadAsStringAsync();

            // Deserializar el JSON a un objeto de dominio auxiliar
            var omdbResponse = JsonConvert.DeserializeObject<OmdbSearchResponse>(json);

            // Lista donde se almacenarán las series mapeadas
            var result = new List<SerieDto>();

            // Recorrer los resultados de la API (si no hay resultados, se usa lista vacía)
            foreach (var item in omdbResponse?.Search ?? [])
            {
                // Mapear cada item a un DTO de Serie
                result.Add(new SerieDto
                {
                    Title = item.Title,
                    Year = item.Year,
                    Poster = item.Poster,
                });
            }

            // Devolver la colección de series
            return result;
        }
    }
}
