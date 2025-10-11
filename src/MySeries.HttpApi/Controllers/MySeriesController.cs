using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MySeries.Application.Contracts;
using MySeries.Application.Contracts.OmdbService;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers;

[Route("api/series")]
[ApiController]
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

    [HttpGet("Get-From-DB-by-title/{title}")]
    public async Task<ActionResult<OmdbSeriesDto>> GetFromDatabaseByTitleAsync(string title)
    {
        var result = await _seriesAppService.GetFromDatabaseByTitleAsync(title);
        return Ok(result);
    }

    [HttpPost("Persist-Database")]
    public async Task<ActionResult<OmdbSeriesDto>> PersistFromOmdbByTitleAsync([FromBody] OmdbSeriesDto omdbDto)
    {
        var result = await _seriesAppService.PersistFromOmdbByTitleAsync(omdbDto);
        return Ok(result);
    }
 }
