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
        [Fact]
        public void CanConvertCodeChallengeStringToSHA256()
        {
            // Arrange
            var shaLength = 64;
            var length = 10;
            var auth = new AuthorizationService();

            // Act
            var code = auth.GenerateRandomString(length);
            var actual = auth.ComputeSHA256(code);

            // Assert
            Assert.Equal(shaLength, actual.Length);
        }
    }
}