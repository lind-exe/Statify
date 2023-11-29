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
using System.Threading.Tasks;
using static Statify.Models.PlayListCollection;

namespace Statify.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task SendSpotifyApiRequestReturnsPlaylistCollection()
        {
            // Arrange
            var expectedResponseJson = File.ReadAllText("../../../Data/playlist.json");
            var expectedPlaylistCollection = JsonSerializer.Deserialize<PlayListCollection>(expectedResponseJson);
            var expectedPlaylistCount = 20;

            var mock = new Mock<ISpotifyService>();

            mock.Setup(x => x.SendRequest<PlayListCollection>(It.IsAny<string>()).Result).Returns(expectedPlaylistCollection!);
            var userService = new UserService(mock.Object);

            // Act
            var actual = await userService.GetPlaylists();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedPlaylistCollection!.Items!.Length, actual!.Items!.Length);
            Assert.Equal(expectedPlaylistCount, actual!.Items!.Length);
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

        //[Fact]
        //public void TestingTracks()
        //{
        //    // Arrange
        //    // This should read from a file.
        //    var json = "";
        //    //var tracks = JsonSerializer.Deserialize<Track>(json);
        //    var expected = new Track();
        //    var mock = new Mock<ISpotifyService>();

        //    //mock.Setup(x => x.SendSpotifyApiRequest<Track>(It.IsAny<string>()).Result).Returns(tracks);
        //    var userService = new UserService(mock.Object);

        //    // Act
        //    var actual = userService.FindForgottenTracks();

        //    // Assert
        //    //actual = expected
        //}
    }
}
