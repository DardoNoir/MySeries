using System;
using System.Collections;

namespace MySeries;

public class Episodio
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    // Clave foránea a Temporada
    public int TemporadaId { get; set; }
    public Temporada Temporada { get; set; } = default!; // Propiedad de navegación

}
