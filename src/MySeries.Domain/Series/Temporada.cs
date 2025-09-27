using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series;

public class Temporada : AggregateRoot<Guid>
{
    public string? Description { get; set; }

    // Relación con Serie
    public Guid SerieId { get; set; }
    public Serie Serie { get; set; } = default!;

    // Relación con Episodios
    public ICollection<Episodio> Episodios { get; set; } = new List<Episodio>();
}