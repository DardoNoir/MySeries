using System;
using System.Collections.Generic;

namespace MySeries;

public class Serie
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public string? Description { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public string? Country { get; set; }
    //ver para temporadas

    public ICollection<Temporada> Temporadas { get; set; } = new List<Temporada>();
    // ver para episodios


}
