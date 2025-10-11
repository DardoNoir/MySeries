using MySeries.Application.Contracts;
using MySeries.Application.Contracts.OmdbService;
using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Application.Series
{
    public class SeriesAppService : ApplicationService, ISeriesAppService
    {
        private readonly IOmdbSeriesService _omdbSeriesService;
        private readonly IRepository<Serie, Guid> _seriesRepository;

        public SeriesAppService(IOmdbSeriesService omdbSeriesService, IRepository<Serie, Guid> seriesRepository)
        {
            _omdbSeriesService = omdbSeriesService;
            _seriesRepository = seriesRepository;
        }

        // Obtener detalle completo de una serie
        public async Task<OmdbSeriesDto> GetFromOmdbAsync(string imdbId)
        {
            return await _omdbSeriesService.GetByImdbIdAsync(imdbId);
        }

 
        // Buscar series por t�tulo y opcionalmente filtrar por g�nero.
        // Devuelve OmdbSeriesSearchDto enriquecido con el g�nero.
 
        public async Task<OmdbSeriesSearchDto> SearchFromOmdbAsync(string title, string? genre = null)
        {
            var searchResult = await _omdbSeriesService.SearchByTitleAsync(title);

            if (searchResult?.Search == null)
                return searchResult ?? new OmdbSeriesSearchDto();

            var filtered = new List<OmdbSeriesSearchItemDto>();

            foreach (var item in searchResult.Search)
            {
                var details = await _omdbSeriesService.GetByImdbIdAsync(item.ImdbId);

                item.Genre = details.Genre; // enriquecemos el SearchItem

                if (string.IsNullOrWhiteSpace(genre) ||
                    (item.Genre != null && item.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)))
                {
                    filtered.Add(item);
                }
            }

            searchResult.Search = filtered;
            searchResult.TotalResults = filtered.Count.ToString();

            return searchResult;
        }

        // --- NEW: Read series info from internal DB by Title ---
        public async Task<OmdbSeriesDto> GetFromDatabaseByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            var serie = await _seriesRepository.FirstOrDefaultAsync(s => s.Title == title);
            if (serie == null)
                throw new InvalidOperationException($"Serie with title '{title}' not found in the database.");

            return MapToDto(serie);
        }

        // --- NEW: Persist OMDb data into DB using Title ---
        public async Task<OmdbSeriesDto> PersistFromOmdbByTitleAsync(OmdbSeriesDto omdbDto)
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
                    TotalSeasons = omdbDto.TotalSeasons
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

                var updated = await _seriesRepository.UpdateAsync(existing, autoSave: true);
                return MapToDto(updated);
            }
        }

        // Helper: map entity -> DTO
        private OmdbSeriesDto MapToDto(Serie s)
        {
            return new OmdbSeriesDto
            {
                //Id = s.Id,
                Title = s.Title,
                Genre = s.Genre,
                Plot = s.Plot,
                Year = s.Year,
                Country = s.Country,
                ImdbId = s.ImdbId,
                ImdbRating = s.ImdbRating,
                TotalSeasons = s.TotalSeasons
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




