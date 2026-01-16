using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;



namespace MySeries.Series
    {   public class SerieAppService : crudAppService<Serie, SerieDto, int , Guid, PagedAndSortedResultRequestDto, CreateUpdateSerieDto>, ISerieAppService
        {   
            private readonly ISeriesApiService _seriesApiService;
            public SerieAppService(IRepository<Serie, int> repository) : base(repository)
                {
                    _seriesApiService =  SeriesApiService;
                }   
    
    
            public async Task<ICollection<SerieDto>> SearchAsync(string title)
                {
                   return await _seriesApiService.SearchSeriesAsync(title);
                }
    
          }
    }
