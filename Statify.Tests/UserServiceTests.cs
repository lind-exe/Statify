using Moq;
using Statify.Interfaces;
using Statify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statify.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void TestingTracks()
        {
            // Arrange
            // This should read from a file.
            var json = "";
            //var tracks = JsonSerializer.Deserialize<Track>(json);
            var expected = new Track();
            var mock = new Mock<ISpotifyService>();

            //mock.Setup(x => x.SendSpotifyApiRequest<Track>(It.IsAny<string>()).Result).Returns(tracks);
            var userService = new UserService(mock.Object);

            // Act
            var actual = userService.FindForgottenTracks();

            // Assert
            //actual = expected
        }
    }
}
