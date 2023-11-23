using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statify.Models;
using System.Text.Json;

namespace Statify.Pages
{
    public class IndexModel : PageModel
    {
        private const string RedirectUri = "https://localhost:7274"; 

        public async Task<IActionResult> OnGetAsync(string code)
        {
            if (code is not null)
            {
                Console.WriteLine($"Received authorization code: {code}");
                var tokenResponse = await ExchangeCodeForTokenAsync(code);
                // Process token response as needed
            }

            return Page();
        }

        private async Task<IActionResult> ExchangeCodeForTokenAsync(string code)
        {
            var authenticatedUser = new PKCEAuthorization();
            using var httpClient = new HttpClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", SpotifyAPICodes.ClientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri },
                { "code_verifier", SpotifyAPICodes.CodeVerifier },
            };

            var content = new FormUrlEncodedContent(tokenRequest);

            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                authenticatedUser = JsonSerializer.Deserialize<PKCEAuthorization>(responseString);
                
            }
            else
            {
                // Handle the error, log the response, etc.
                
            }
            return Page();
        }
    }
}
