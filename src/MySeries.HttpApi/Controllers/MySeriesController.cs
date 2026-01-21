using Microsoft.AspNetCore.Mvc;
using MySeries.Application.Contracts;
using MySeries.SerieService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers
{
    [Route("api/app/serie")]
    public class SerieController : AbpController
    {
        private readonly ISeriesAppService _seriesAppService;

        public SerieController(ISeriesAppService seriesAppService)
        {
            _seriesAppService = seriesAppService;
        }

        [HttpGet("search-by-title")]
        public async Task<ICollection<SerieDto>> SearchByTitleAsync([FromQuery] string title)
        {
            return await _seriesAppService.SearchByTitleAsync(title);
        }
    }
}
