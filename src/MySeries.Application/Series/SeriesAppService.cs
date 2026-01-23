using MySeries.Application.Contracts;
using MySeries.SerieService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;


namespace MySeries.Series
{
    // Servicio de aplicación para gestionar entidades Serie
    // Hereda de CrudAppService para obtener operaciones CRUD estándar
    public class SerieAppService
        : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto>,
          ISeriesAppService
    {
        // Servicio externo para consultar series por API
        private readonly ISeriesApiService _seriesApiService;

        // Constructor con inyección del repositorio y el servicio de API
        public SerieAppService(
            IRepository<Serie, int> repository,
            ISeriesApiService seriesApiService
        ) : base(repository)
        {
            _seriesApiService = seriesApiService;
        }

        // Busca series por título utilizando el servicio externo
        [RemoteService(IsEnabled = false)]
        public async Task<ICollection<SerieDto>> SearchByTitleAsync(string title, string? genre)
        {
            // Delegar la búsqueda al servicio de integración
            return await _seriesApiService.GetSeriesAsync(title, genre);
        }


        // Guarda una serie
        public async Task<SerieDto> SaveAsync(SerieDto input)
        {
            // Evitar duplicados por IMDbId
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


        public async Task<SerieDto> GetOrCreateFromApiAsync(string imdbId)
        {
            var existing = await Repository.FirstOrDefaultAsync(s => s.ImdbId == imdbId);
            if (existing != null)
                return ObjectMapper.Map<Serie, SerieDto>(existing);

            var serieFromApi = await _seriesApiService.GetSerieByImdbIdAsync(imdbId);
            if (serieFromApi == null)
            throw new BusinessException("SerieNotFoundInApi");

            var entity = ObjectMapper.Map<SerieDto, Serie>(serieFromApi);
            await Repository.InsertAsync(entity, autoSave: true);
            return ObjectMapper.Map<Serie, SerieDto>(entity);
        }
    }
}
