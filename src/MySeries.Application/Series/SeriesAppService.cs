using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MySeries.Application.Contracts;
using MySeries.Series;
using MySeries.SerieService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;

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
        public async Task<ICollection<SerieDto>> SearchByTitleAsync(string title)
        {
            // Delegar la búsqueda al servicio de integración
            return await _seriesApiService.GetSeriesAsync(title);
        }
    }
}
