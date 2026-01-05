using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using MySeries.SerieService;

namespace MySeries.Application.Contracts
{
    public interface ISeriesAppService : ICrudAppService<serieDto,int,
        PagedAndSortedResultRequestDto>
    {
        Task<serieDto> GetFromOmdbAsync(string imdbId);
       Task<ICollection<serieDto>> SearchFromOmdbAsync
        (string title, string? genre = null);

        Task<serieDto> GetFromDatabaseByTitleAsync(string title);
        Task<serieDto> PersistFromOmdbByTitleAsync(serieDto omdbSeriesDto);

    }
}
