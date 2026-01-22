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
    }
}
