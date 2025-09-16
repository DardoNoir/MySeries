using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series;

public class Serie : AggregateRoot<Guid>
{
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public string? Description { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public string? Country { get; set; }
     public string? ImdbId { get; set; }  // relación con OMDb
    //ver para temporadas

    public ICollection<Temporada> Temporadas { get; set; } = new List<Temporada>();


}
