using Moq;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Statify.Models.TrackData;

namespace Statify.Tests
{
    public class StatisticServiceTests
    {
        [Fact]
        public void CanArrangeGenreFrequencyForTopList()
        {
            // Arrange
            var statisticsService = new StatisticsService(null);

            var artists = new List<Artist>
        {
            new Artist { Id = "1", Genres = new List<string> { "Rock", "Pop" } },
            new Artist { Id = "2", Genres = new List<string> { "Pop", "Jazz" } },
            new Artist { Id = "3", Genres = new List<string> { "Rock", "Country" } }
        };

            var genreDictionary = new Dictionary<string, int>
        {
            { "Rock", 0 },
            { "Pop", 0 },
            { "Jazz", 0 },
            { "Country", 0 }
        };

            // Act
            statisticsService.ArrangesGenreFrequencyForTopList(artists, genreDictionary);

            // Assert
            Assert.Equal(2, genreDictionary["Rock"]);
            Assert.Equal(2, genreDictionary["Pop"]);
            Assert.Equal(1, genreDictionary["Jazz"]);
            Assert.Equal(1, genreDictionary["Country"]);
        }

        [Fact]
        public void CanCompareAndCalculateScoreWithCorrectResult()
        {
            // Arrange
            var statisticsService = new StatisticsService(null); 

            var genresFromTopArtists = new Dictionary<string, int>
        {
            { "Rock", 8 },
            { "Pop", 5 },
            { "Jazz", 10 },
        };

            var genresFromLikedSongs = new Dictionary<string, int>
        {
            { "Rock", 37 },
            { "Pop", 20 },
        };

            statisticsService.GenresFromTopArtists = genresFromTopArtists;
            statisticsService.GenresFromLikedSongs = genresFromLikedSongs;

            // Act
            var actual = statisticsService.CompareAndCalculateScore();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(3, actual.Result.Keys.Count); 
            Assert.Equal(296, actual.Result["Rock"]); // Expected result: 8 * 37 = 296
            Assert.Equal(100, actual.Result["Pop"]);  // Expected result: 5 * 20 = 100
            Assert.Equal(10, actual.Result["Jazz"]);  // No matching genre in GenresFromLikedSongs, so should remain the same (10)
        }
    }
}
