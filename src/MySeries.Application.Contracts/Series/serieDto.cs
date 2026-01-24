using System.Text.Json.Serialization;
using Volo.Abp.Application.Dtos;

namespace MySeries.SerieService
{
public class SerieDto : EntityDto<int>
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Year { get; set; }
    public string? Poster { get; set; }

    public string? Plot { get; set; }
    public string? Country { get; set; }
    public required string ImdbId { get; set; }
    public string? ImdbRating { get; set; }
    public string? TotalSeasons { get; set; }
    public string? Runtime { get; set; }
    public string? Actors { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
}

}
