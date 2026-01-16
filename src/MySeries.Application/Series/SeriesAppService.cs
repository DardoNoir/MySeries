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
    public class SerieAppService : CrudAppService<Serie, serieDto, int, PagedAndSortedResultRequestDto, serieDto, serieDto>, ISeriesAppService, ITransientDependency
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _serieRepository;
        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService) : base(repository)
        {
            _seriesApiService =  seriesApiService;
            _serieRepository = repository;

        }

        public async Task<ICollection<serieDto>> SearchByTitleAsync(string title)
        {
            return await _seriesApiService.GetSeriesAsync(title);
        }

    }
}
