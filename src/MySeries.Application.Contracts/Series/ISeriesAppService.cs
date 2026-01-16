using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using MySeries.SerieService;

namespace MySeries.Application.Contracts
{
    public interface ISeriesAppService : ICrudAppService<serieDto,int, PagedAndSortedResultRequestDto>
    {
        Task<ICollection<serieDto>> SearchByTitleAsync(string title);

    }
}

