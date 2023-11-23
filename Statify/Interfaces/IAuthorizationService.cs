using Statify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Statify.Interfaces
{
    public interface IAuthorizationService
    {
        public void GenerateCodeChallenge();

        public string GenerateRandomString(int length);

        public string GenerateCodeChallenge(string codeVerifier);
    }
}
