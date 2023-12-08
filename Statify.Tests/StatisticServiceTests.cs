using Moq;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            var statisticsService = new StatisticsService(null!);

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
            var statisticsService = new StatisticsService(null!); 

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
            Assert.Equal(3, actual.Keys.Count); 
            Assert.Equal(296, actual["Rock"]); // Expected result: 8 * 37 = 296
            Assert.Equal(100, actual["Pop"]);  // Expected result: 5 * 20 = 100
            Assert.Equal(10, actual["Jazz"]);  // No matching genre in GenresFromLikedSongs, so should remain the same (10)
        }

        [Fact]
        public void CanCalculateTotalDurationOfTopSongsReturnsCorrectDuration()
        {
            // Arrange
            var tracklist = new TrackData.TrackList
            {
                Tracks = new List<Track>
                {
                    new Track { DurationMs = 60000 }, // 60 seconds
                    new Track { DurationMs = 150000 }, // 150 seconds
                    new Track { DurationMs = 2500000 }, // 2500 seconds
                    new Track { DurationMs = 200000 }, // 200 seconds
                    new Track { DurationMs = 1500000 }, // 1500 seconds
                }
            };
            TimeSpan expectedTime = TimeSpan.Parse("01:13:30");

            var trackData = new StatisticsService(null!);

            // Act
            var actual = trackData.CalculateTotalDurationOfTopSongs(tracklist);

            // Assert
            Assert.Equal(1, actual.Hours); // Expected total hours
            Assert.Equal(13, actual.Minutes); // Expected total minutes
            Assert.Equal(30, actual.Seconds); // Expected total seconds
            Assert.Equal(expectedTime, actual); // Expected total seconds

        }
        [Fact]
        public async Task GetUserSoundProfileReturnsExpectedSoundProfile()
        {
            // Arrange
            var audioFeatureJson = File.ReadAllText("../../../Data/top50audiofeature.json");
            var expectedAudioFeature = JsonSerializer.Deserialize<AudioFeatureCollection>(audioFeatureJson);

            var trackJson = File.ReadAllText("../../../Data/top50tracks.json");
            var expectedTrack = JsonSerializer.Deserialize<TrackData.TrackList>(trackJson);

            var mockUserService = new Mock<IUserService>();
            var mockService = new Mock<StatisticsService>(mockUserService.Object);

            mockUserService.Setup(x => x.GetTopItems<TrackData.TrackList>("tracks", "long_term", It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(expectedTrack!);
            mockUserService.Setup(x => x.GetAudioFeatures(It.IsAny<string>()))
                           .ReturnsAsync(expectedAudioFeature!);

            var userService = mockService.Object;


            // Act
            var actualSoundProfile = await userService.GetUserSoundProfileTitle();

            // Assert
            Assert.NotNull(actualSoundProfile);
            Assert.Equal("red bull enjoyer", actualSoundProfile); 
        }
    }
}
