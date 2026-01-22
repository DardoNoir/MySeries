using System.Text.Json.Serialization;
using Volo.Abp.Application.Dtos;

namespace MySeries.SerieService
{
    public class SerieDto : EntityDto<int>
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Poster { get; set; }
        public string? Genre { get; set; }
    }
}
