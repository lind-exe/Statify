using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Statify.Pages
{
    public class IndexModel : PageModel
    {
        private const string RedirectUri = "https://localhost:7274"; // Should match the redirect URI used in AuthorizeModel
        private const string ClientId = "de733e5c3f6b418a97c787f8abe82ba5";

        public async Task<IActionResult> OnGetAsync(string code)
        {
            if (code is not null)
            {
                Console.WriteLine($"Received authorization code: {code}");
                var tokenResponse = await ExchangeCodeForTokenAsync(code);
                // Process token response as needed
                return Content(tokenResponse); // Return response content for demonstration purposes
            }

            return Page();
        }

        private async Task<string> ExchangeCodeForTokenAsync(string code)
        {
            string codeVerifier = HttpContext.Session.GetString("code_verifier");
            string codeChallenge = HttpContext.Session.GetString("code_challenge");
            
            using var httpClient = new HttpClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri },
                { "code_verifier", codeVerifier },
            };

            var content = new FormUrlEncodedContent(tokenRequest);

            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error, log the response, etc.
                return $"Token request failed with status code: {response.StatusCode}";
            }
        }
    }
}
