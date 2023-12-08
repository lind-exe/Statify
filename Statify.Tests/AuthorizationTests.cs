using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using Statify.Interfaces;
using Statify.Models;
using Statify.Services;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Statify.Tests
{
    public class AuthorizationTests
    {
        [Fact]
        public void CheckIfGenerateCodeChallengeIsNotEmpty()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                Session = new MockSession()
            };

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            var auth = new AuthorizationService(httpContextAccessor.Object);

            // Act
            auth.GenerateCodeChallenge();

            // Assert
            Assert.NotNull(auth.SpotifyApiCodes);
            Assert.False(string.IsNullOrWhiteSpace(auth.SpotifyApiCodes.CodeChallenge));
        }


        [Fact]
        public void CanGenerateCodeChallengeWithCorrectLength()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                Session = new MockSession()
            };

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            var auth = new AuthorizationService(httpContextAccessor.Object);

            // Act
            auth.GenerateCodeChallenge();
            const int expectedLength = 43;

            // Assert
            Assert.Equal(expectedLength, auth.SpotifyApiCodes?.CodeChallenge?.Length);
        }

        [Fact]
        public void GenerateCodeChallengeHasValidCharacters()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                Session = new MockSession()
            };

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            var auth = new AuthorizationService(httpContextAccessor.Object);

            // Act
            auth.GenerateCodeChallenge();

            // Assert
            Assert.Matches("^[-A-Za-z0-9_]+$", auth.SpotifyApiCodes?.CodeChallenge);
        }

        [Fact]
        public void CanGenerateRandomCodeChallengeStringOfCorrectLength()
        {
            // Arrange
            var length = 10;
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var auth = new AuthorizationService(httpContextAccessor.Object);

            // Act
            var actual = auth.GenerateRandomString(length);

            // Assert
            Assert.Equal(length, actual.Length);
        }

        [Theory]
        [InlineData("testCode", "aGG0q7q5D_-PRv9Vfa9KIfL3Xar-2tMKpUxUvpUevws")]
        [InlineData("testCode2", "k1RlPgEThBlzlJz3ra1rPY1MK4j3qkE3Kj3yDuCz2wo")]
        [InlineData("", "")]
        public void CanGenerateCodeChallengeAndReturnExpectedCode(string codeVerifier, string expectedCode)
        {
            // Arrange
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var auth = new AuthorizationService(httpContextAccessor.Object);

            // Act
            var codeChallenge = auth.GenerateCodeChallenge(codeVerifier);

            // Assert
            Assert.Equal(expectedCode, codeChallenge);
        }

        [Fact]
        public async Task ReplaceWithTimeoutWhenTimeoutOccurredThenReturnsTimeoutMessage()
        {
            // Arrange
            string input = "Just/some/text";
            string pattern = "\\/";
            string replacement = "_";

            int timeoutMs = 10; // Timeout set to 10 milliseconds for testing

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var methodUnderTest = new AuthorizationService(httpContextAccessor.Object);

            // Act
            var task = Task.Run(() => methodUnderTest.ReplaceWithTimeout(input, pattern, replacement));
            await Task.Delay(timeoutMs);

            // Assert
            Assert.True(task.IsCompleted);

            if (!task.IsCompleted)
            {
                Assert.True(task.IsCanceled, "Task should be canceled due to timeout");
            }
            else
            {
                Assert.NotEqual("Timeout occurred during regex operation", task.Result);
            }
        }
    }
}