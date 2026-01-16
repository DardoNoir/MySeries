using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MySeries.Application.Contracts;
using Volo.Abp.AspNetCore.Mvc;
using MySeries.SerieService;
using System.Collections;

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
    public async Task<SerieDto> GetFromOmdbAsync(string imdbId)
    {
        return await _seriesAppService.GetFromOmdbAsync(imdbId);
    }

   // [HttpGet("search")]
   // public async Task<ICollection<serieDto>> SearchFromOmdbAsync([FromQuery] string title, [FromQuery] string? genre = null)
   // {
    //    return await _seriesAppService.SearchFromOmdbAsync(title, genre);
 //   }

    [HttpGet("Get-From-DB-by-title/{title}")]
    public async Task<ActionResult<SerieDto>> GetFromDatabaseByTitleAsync(string title)
    {
        var result = await _seriesAppService.GetFromDatabaseByTitleAsync(title);
        return Ok(result);
    }

    [HttpPost("Persist-Database")]
    public async Task<ActionResult<SerieDto>> PersistFromOmdbByTitleAsync([FromBody] SerieDto omdbDto)
    {
        var result = await _seriesAppService.PersistFromOmdbByTitleAsync(omdbDto);
        return Ok(result);
    }
 }
