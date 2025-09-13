using System;
using System.Collections;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series;

public class Episodio : AggregateRoot<Guid>
{
   // public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    // Clave foránea a Temporada
    public int TemporadaId { get; set; }
    public Temporada Temporada { get; set; } = default!; // Propiedad de navegación

}
