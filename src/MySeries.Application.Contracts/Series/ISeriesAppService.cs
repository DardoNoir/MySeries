using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using MySeries.SerieService;

namespace MySeries.Application.Contracts
{
    public interface ISeriesAppService : ICrudAppService<SerieDto,int, PagedAndSortedResultRequestDto>
    {
        Task<ICollection<SerieDto>> SearchByTitleAsync(string title, string? genre);

        Task<SerieDto> SaveAsync(SerieDto input);

    }
}

