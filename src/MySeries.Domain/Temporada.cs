using System;
using System.Collections.Generic;

namespace MySeries;

public class Temporada
{
    public required int Id { get; set; }
    public string? Description { get; set; }

    // Relación con la entidad Serie y Episodio

    public Guid SerieId { get; set; }
    public Serie Serie { get; set; } = default!; // Propiedad de navegación
    public ICollection<Episodio> Episodios { get; set; } = new List<Episodio>();
}
