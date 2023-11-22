using System.Security.Cryptography;
using System.Text;

namespace Statify.Interfaces
{
    public interface IAuthorizationService
    {
		public string GenerateRandomString(int length);
		public string ComputeSHA256(string plain);
		public string Base64EncodeSHA256(string sha256Hash);
		public string GenerateCodeChallenge();
	}
}
