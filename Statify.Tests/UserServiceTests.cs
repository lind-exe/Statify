using Moq;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;


namespace Statify.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task SendSpotifyApiRequestReturnsPlaylistCollection()
        {
            // Arrange
            var expectedResponseJson = File.ReadAllText("../../../Data/playlist.json");
            var expectedPlaylistCollection = JsonSerializer.Deserialize<PlaylistResponse>(expectedResponseJson);
            var expectedPlaylistCount = 20;

            var mock = new Mock<ISpotifyService>();

            mock.Setup(x => x.SendRequest<PlaylistResponse>(It.IsAny<string>()).Result).Returns(expectedPlaylistCollection!);
            var userService = new UserService(mock.Object);

            // Act
            var actual = await userService.GetPlaylists();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedPlaylistCollection!.Items!.Count, actual!.Items!.Count);
            Assert.Equal(expectedPlaylistCount, actual!.Items!.Count);
        }

        [Fact]
        public async Task SendSpotifyApiRequestReturnsUser()
        {
            // Arrange
            var expectedResponseJson = File.ReadAllText("../../../Data/user.json");
            var expectedUser = JsonSerializer.Deserialize<User>(expectedResponseJson);

            var mock = new Mock<ISpotifyService>();

            mock.Setup(x => x.SendRequest<User>(It.IsAny<string>()).Result).Returns(expectedUser!);
            var userService = new UserService(mock.Object);

            // Act
            var actual = await userService.GetUser();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedUser!.Id, actual.Id);
        }
        [Fact]
        public async Task SendSpotifyApiRequestReturnsTopTracks()
        {
            // Arrange
            var expectedCount = 20;
            var expectedResponseJson = File.ReadAllText("../../../Data/toptracks.json");
            var expectedTracks = JsonSerializer.Deserialize<TrackData.TrackList>(expectedResponseJson);

            var mock = new Mock<ISpotifyService>();

            mock.Setup(x => x.SendRequest<TrackData.TrackList>(It.IsAny<string>()).Result).Returns(expectedTracks!);
            var userService = new UserService(mock.Object);

            // Act
            var actual = await userService.GetTopItems<TrackData.TrackList>("tracks", "medium_term");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedTracks!.Tracks![0].Id, actual.Tracks![0].Id);
            Assert.Equal(expectedCount, actual.Tracks.Count);
        }
        [Fact]
        public async Task SendSpotifyApiRequestReturnsTopArtists()
        {
            // Arrange
            var expectedCount = 20;
            var expectedResponseJson = File.ReadAllText("../../../Data/topartists.json");
            var expectedArtists = JsonSerializer.Deserialize<ArtistCollection>(expectedResponseJson);

            var mock = new Mock<ISpotifyService>();

            mock.Setup(x => x.SendRequest<ArtistCollection>(It.IsAny<string>()).Result).Returns(expectedArtists!);
            var userService = new UserService(mock.Object);

            // Act
            var actual = await userService.GetTopItems<ArtistCollection>("tracks", "medium_term");

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedArtists!.Artists![0].Id, actual.Artists![0].Id);
            Assert.Equal(expectedCount, actual.Artists.Count);
        }
    }
}
