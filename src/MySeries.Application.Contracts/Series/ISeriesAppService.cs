using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using MySeries.SerieService;

namespace MySeries.Application.Contracts
{
    public interface ISeriesAppService : ICrudAppService<SerieDto,int,
        PagedAndSortedResultRequestDto>
    {
        Task<SerieDto> GetFromOmdbAsync(string imdbId);
       Task<ICollection<SerieDto    >> SearchFromOmdbAsync
        (string title, string? genre = null);

        Task<SerieDto> GetFromDatabaseByTitleAsync(string title);
        Task<SerieDto> PersistFromOmdbByTitleAsync(SerieDto omdbSeriesDto);

    }
}
