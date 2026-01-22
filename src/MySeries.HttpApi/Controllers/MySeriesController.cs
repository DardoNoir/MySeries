using Microsoft.AspNetCore.Mvc;
using MySeries.Application.Contracts;
using MySeries.Series;
using MySeries.SerieService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers
{
    [Route("api/app/serie")]
    public class SerieController : AbpController
    {
        private readonly SerieAppService _serieAppService;

        public SerieController(SerieAppService serieAppService)
        {
            _serieAppService = serieAppService;
        }

        [HttpGet("search-by-title")]
        public async Task<ICollection<SerieDto>> SearchByTitleAsync([FromQuery] string title, [FromQuery] string? genre)
        {
            return await _serieAppService.SearchByTitleAsync(title, genre);
        }
    }
}
