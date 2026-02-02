using MySeries.Application.Contracts;
using MySeries.SerieService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;


namespace MySeries.Series
{
    public class SerieAppService
        : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto>,
          ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;

        public SerieAppService(
            IRepository<Serie, int> repository,
            ISeriesApiService seriesApiService
        ) : base(repository)
        {
            _seriesApiService = seriesApiService;
        }

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)]
        // Búsqueda de Series por Título, con opción de filtrar por Género
        public async Task<ICollection<SerieDto>> SearchByTitleAsync(string title, string? genre)
        {
            return await _seriesApiService.GetSeriesAsync(title, genre);
        }


        // Guardar una serie en la DB
        public async Task<SerieDto> SaveAsync(SerieDto input)
        {
            var existing = await Repository.FirstOrDefaultAsync(
                x => x.ImdbId == input.ImdbId);
            
            if (existing != null)
            {
                throw new BusinessException("SerieAlreadyExists")
                    .WithData("ImdbId", input.ImdbId);
            }

            var serie = new Serie
            {
                Title = input.Title!,
                Genre = input.Genre!,
                Year = input.Year!,
                Poster = input.Poster,
                Plot = input.Plot,
                Country = input.Country,
                ImdbId = input.ImdbId,
                ImdbRating = input.ImdbRating,
                TotalSeasons = input.TotalSeasons,
                Runtime = input.Runtime,
                Actors = input.Actors,
                Director = input.Director,
                Writer = input.Writer
            };

            await Repository.InsertAsync(serie, autoSave: true);

            return ObjectMapper.Map<Serie, SerieDto>(serie);
        }

        // Obtener series de la DB o API --> Usado en las Watchlists
        public async Task<SerieDto> GetOrCreateFromApiAsync(string imdbId)
        {
            // Si la Serie se encuentra en la DB
            var existing = await Repository.FirstOrDefaultAsync(s => s.ImdbId == imdbId);
            if (existing != null)
                return ObjectMapper.Map<Serie, SerieDto>(existing);

            // Si la Serie NO se encuentra en la DB
            var serieFromApi = await _seriesApiService.GetSerieByImdbIdAsync(imdbId);
            if (serieFromApi == null)
            throw new BusinessException("SerieNotFoundInApi");

            // Guardar en la DB la Serie buscada en la API
            var entity = ObjectMapper.Map<SerieDto, Serie>(serieFromApi);
            await Repository.InsertAsync(entity, autoSave: true);
            return ObjectMapper.Map<Serie, SerieDto>(entity);
        }

        // Busca series almacenadas en la base de datos por título
        [RemoteService(IsEnabled = false)]
        public async Task<List<SerieDto>> SearchInDbByTitleAsync(string title)
        {
            // Normalizamos el texto para evitar problemas de mayúsculas/minúsculas
            title = title.Trim();

            // Consulta a la DB
            var queryable = await Repository.GetQueryableAsync();

            var series = queryable
            .Where(s => s.Title.ToLower().Contains(title.ToLower()))
            .ToList();


            return ObjectMapper.Map<List<Serie>, List<SerieDto>>(series);
        }

    }
}
