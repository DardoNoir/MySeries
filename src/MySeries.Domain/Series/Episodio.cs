using System;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series;

public class Episodio : AggregateRoot<Guid>
{
    public required string Title { get; set; }
    public string? Description { get; set; }

    // Clave foránea a Temporada
    public Guid TemporadaId { get; set; }
    public Temporada Temporada { get; set; } = default!;
}
