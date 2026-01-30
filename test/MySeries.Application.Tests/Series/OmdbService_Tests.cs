using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySeries.Series
{
    public sealed class OmdbService_Tests
    {
        private readonly OmdbService _service;

        public OmdbService_Tests()
        {
            _service = new OmdbService();
        }

        // Test Para Verificar Búsqueda de una Serie por Nombre (sin género)
        [Fact]
        public async Task ShouldSearchOneSerie()
        {
            // arrange
            var serieName = "Breaking Bad";
            var genre = null as string;

            // act
            var result = await _service.GetSeriesAsync(serieName, genre);

            // assert
            result.Count.ShouldBeGreaterThan(0);
            result.ShouldNotBeNull();
            result.ShouldContain(b => b.Title == serieName);
        }

        // Test Para Verificar Búsqueda de Series por Nombre y Género
        [Fact]
        public async Task ShouldSearchSeriesByGenre()
        {
            // arrange
            var serieName = "Friends";
            var genre = "Comedy";
            // act
            var result = await _service.GetSeriesAsync(serieName, genre);
            // assert
            result.Count.ShouldBeGreaterThan(0);
            result.ShouldNotBeNull();
            result.ShouldAllBe(b => b.Genre != null && b.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase));
        }

        // Test Para Verificar Búsqueda de Series por Nombre y Género que No Existen
        [Fact]
        public async Task ShouldReturnEmptyIfNoSeriesFoundForGenre()
        {
            // arrange
            var serieName = "sjsjsjsjsjsj";
            var genre = null as string;
            // act
            var result = await _service.GetSeriesAsync(serieName, genre);
            // assert
            result.Count.ShouldBe(0);
        }
    }
}
