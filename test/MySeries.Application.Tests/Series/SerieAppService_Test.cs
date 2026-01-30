using AutoMapper.Internal.Mappers;
using MySeries.SerieService;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;



namespace MySeries.Series
{
    public sealed class SerieAppService_Test  
    {
        private readonly SerieAppService _service;
        private readonly IRepository<Serie, int> _serieRepo;
        private readonly ISeriesApiService _seriesApiService;

        public SerieAppService_Test()
        {
            _serieRepo = Substitute.For<IRepository<Serie, int>>();
            _seriesApiService = Substitute.For<ISeriesApiService>();
            _service = new SerieAppService(
                _serieRepo,
                _seriesApiService
            );
        }

        [Fact]
        public async Task ShouldSearchSeriesByTitle()
        {
            var title = "Inception";
            var genre = "Sci-Fi";
            var expectedSeries = new List<SerieDto>
            {
                new SerieDto { Title = "Inception", Genre = "Sci-Fi", Year = "2010", ImdbId = "tt1375666" }
            };
            _seriesApiService
                .GetSeriesAsync(title, genre)
                .Returns(Task.FromResult((ICollection<SerieDto>)expectedSeries));
            var result = await _service.SearchByTitleAsync(title, genre);
            Assert.Equal(expectedSeries.Count, result.Count);
            Assert.Equal(expectedSeries.First().Title, result.First().Title);
        }

        [Fact]
       // Test para buscar una serie en la DB que no exista
       public async Task ShouldSaveSerie_WhenNotExists()
        {
            var newSerie = new SerieDto
            {
                Title = "The Matrix",
                Genre = "Sci-Fi",
                Year = "1999",
                ImdbId = "tt0133093",
                Poster = "someposterurl",
                Plot = "A computer hacker learns about the true nature of his reality."
            };
            _serieRepo
                .FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Serie, bool>>>())
                .Returns(Task.FromResult<Serie?>(null));
            var savedSerie = await _service.SaveAsync(newSerie);
            Assert.Equal(newSerie.Title, savedSerie.Title);
            Assert.Equal(newSerie.ImdbId, savedSerie.ImdbId);
        }
    }
}
