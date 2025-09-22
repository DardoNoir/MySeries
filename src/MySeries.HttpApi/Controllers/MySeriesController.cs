using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MySeries.Application.Contracts;
using MySeries.Application.Contracts.OmdbService;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers;

[Route("api/series")]
public class SeriesController : AbpController
{
    private readonly ISeriesAppService _seriesAppService;

    public SeriesController(ISeriesAppService seriesAppService)
    {
        _seriesAppService = seriesAppService;
    }

    [HttpGet("{imdbId}")]
    public async Task<OmdbSeriesDto> GetFromOmdbAsync(string imdbId)
    {
        return await _seriesAppService.GetFromOmdbAsync(imdbId);
    }

    [HttpGet("search")]
    public async Task<OmdbSeriesSearchDto> SearchFromOmdbAsync([FromQuery] string title, [FromQuery] string? genre = null)
    {
        return await _seriesAppService.SearchFromOmdbAsync(title, genre);
    }
}
