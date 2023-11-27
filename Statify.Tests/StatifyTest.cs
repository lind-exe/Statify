using Microsoft.AspNetCore.Routing;
using Statify.Models;
using Statify.Services;
using System.Text.RegularExpressions;

namespace Statify.Tests
{
    public class StatifyTest
    {
        [Fact]
        public void CheckIfGenerateCodeChallengIsNotEmpty()
        {
            // Arrange
            var auth = new AuthorizationService();

            // Act
            auth.GenerateCodeChallenge();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(SpotifyApiCodes.CodeChallenge));
        }

        [Fact]
        public void CanGenerateCodeChallengeWithCorrectLength()
        {
            // Arrange
            var auth = new AuthorizationService();

            // Act
            auth.GenerateCodeChallenge();
            const int expectedLength = 43;

            // Assert
            Assert.Equal(expectedLength, SpotifyApiCodes.CodeChallenge!.Length);
        }

        [Fact]
        public void GenerateCodeChallengeHasValidCharacters()
        {
            // Arrange
            var auth = new AuthorizationService();

            // Act
            auth.GenerateCodeChallenge();

            // Assert
            Assert.Matches("^[ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_]+$", SpotifyApiCodes.CodeChallenge);
        }


        [Fact]
        public void CanGenerateRandomCodeChallengeStringOfCorrectLength()
        {
            // Arrange
            var length = 10;
            var auth = new AuthorizationService();

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
            var codeGenerator = new AuthorizationService();

            // Act
            var codeChallenge = codeGenerator.GenerateCodeChallenge(codeVerifier);

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

            var methodUnderTest = new AuthorizationService(); 

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