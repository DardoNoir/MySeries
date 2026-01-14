using MySeries.Application.Contracts;
using MySeries.Application.Contracts.OmdbService;
using MySeries.Series;
using MySeries.SerieService;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Application.Series
{
    public class SeriesAppService : ApplicationService
    {
        private readonly IOmdbSeriesService _omdbSeriesService;
        private readonly IRepository<Serie, Guid> _seriesRepository;

        public SeriesAppService(IOmdbSeriesService omdbSeriesService, IRepository<Serie, Guid> seriesRepository)
        {
            _omdbSeriesService = omdbSeriesService;
            _seriesRepository = seriesRepository;
        }

        // Obtener detalle completo de una serie
        public async Task<SerieDto> GetFromOmdbAsync(string imdbId)
        {
            return await _omdbSeriesService.GetByImdbIdAsync(imdbId);
        }

 
        // Buscar series por titulo y opcionalmente filtrar por genero.
        // Devuelve OmdbSeriesSearchDto enriquecido con el genero.

        // --- NEW: Read series info from internal DB by Title ---
        public async Task<serieDto> GetFromDatabaseByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Title == title);
            if (serie == null)
                throw new InvalidOperationException($"Serie with title '{title}' not found in the database.");

            return MapToDto(serie);
        }

        // --- NEW: Persist OMDb data into DB using Title ---
        public async Task<serieDto> PersistFromOmdbByTitleAsync(serieDto omdbDto)
        {
            if (omdbDto is null)
                throw new ArgumentNullException(nameof(omdbDto));

            if (string.IsNullOrWhiteSpace(omdbDto.Title))
                throw new ArgumentException("OMDb DTO must contain a Title", nameof(omdbDto));

            // Find existing record by Title
            var existing = await _seriesRepository.FirstOrDefaultAsync(s => s.Title == omdbDto.Title);

            DateTime releaseDate = ParseYearToDate(omdbDto.Year);

            if (existing == null)
            {
                // Create new Serie entity
                var newSerie = new Serie
                {
                    Title = omdbDto.Title,
                    Genre = omdbDto.Genre ?? "Unknown",
                    Plot = omdbDto.Plot,
                    Year = omdbDto.Year,
                    Country = omdbDto.Country,
                    ImdbId = omdbDto.ImdbId,
                    ImdbRating = omdbDto.ImdbRating,
                    TotalSeasons = omdbDto.TotalSeasons,
                    Poster = omdbDto.Poster,
                    Runtime = omdbDto.Runtime,      
                    Actors = omdbDto.Actors,
                    Director = omdbDto.Director,
                    Writer = omdbDto.Writer
                };

                var inserted = await _seriesRepository.InsertAsync(newSerie, autoSave: true);
                return MapToDto(inserted);
            }
            else
            {
                // Update existing Serie fields
                existing.Genre = omdbDto.Genre ?? existing.Genre;
                existing.Plot = omdbDto.Plot ?? existing.Plot;
                existing.Year = omdbDto.Year;
                existing.Country = omdbDto.Country ?? existing.Country;
                existing.ImdbId = omdbDto.ImdbId ?? existing.ImdbId;
                existing.ImdbRating = omdbDto.ImdbRating ?? existing.ImdbRating;
                existing.TotalSeasons = omdbDto.TotalSeasons ?? existing.TotalSeasons;
                existing.Poster = omdbDto.Poster ?? existing.Poster;
                existing.Runtime = omdbDto.Runtime ?? existing.Runtime;
                existing.Actors = omdbDto.Actors ?? existing.Actors;
                existing.Director = omdbDto.Director ?? existing.Director;
                existing.Writer = omdbDto.Writer ?? existing.Writer;

                var updated = await _seriesRepository.UpdateAsync(existing, autoSave: true);
                return MapToDto(updated);
            }
        }

        // Helper: map entity -> DTO
        private serieDto MapToDto(Serie s)
        {
            return new serieDto
            {
                Title = s.Title,
                Genre = s.Genre,
                Plot = s.Plot,
                Year = s.Year,
                Country = s.Country,
                ImdbId = s.ImdbId ?? string.Empty,
                ImdbRating = s.ImdbRating,
                TotalSeasons = s.TotalSeasons,
                Runtime = s.Runtime,
                Poster = s.Poster,
                Actors = s.Actors,
                Director = s.Director,
                Writer = s.Writer
            };
        }

        // Helper: extract first 4-digit year from OMDb "Year" field
        private DateTime ParseYearToDate(string? year)
        {
            if (string.IsNullOrWhiteSpace(year))
                return DateTime.UtcNow;

            var match = Regex.Match(year, @"\d{4}");
            if (match.Success && int.TryParse(match.Value, out int y) && y > 1900)
            {
                return new DateTime(y, 1, 1);
            }

            return DateTime.UtcNow;
        }
    }
}




