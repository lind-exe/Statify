using Statify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Statify.Interfaces
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Initializes codechallenge generation process
        /// </summary>
        public void GenerateCodeChallenge();
        /// <summary>
        /// Generates a random string with specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Random string</returns>
        public string GenerateRandomString(int length);
        /// <summary>
        /// Generates a codechallenge with provided randomized string and then securily encodes into sha256 and then base64
        /// </summary>
        /// <param name="codeVerifier"></param>
        /// <returns>Encoded base64 string</returns>
        public string GenerateCodeChallenge(string codeVerifier);
        /// <summary>
        /// Combines Spotify uri with neccessary url parameters using stored session data
        /// </summary>
        /// <returns>Redirect url</returns>
        public string GenerateQueryParams();
    }
}
