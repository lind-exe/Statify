using Statify.Services;

namespace Statify.Tests
{
    public class StatifyTest
    {
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
    }
}