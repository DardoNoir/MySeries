using System;

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
    // ver para episodios

}
