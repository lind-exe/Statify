using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statify.Interfaces;
using Statify.Models;
using System.Text;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace Statify.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Interfaces.IAuthorizationService _authorizationService;
        private const string RedirectUri = "https://localhost:7274";
        public PkceAuthorization? Authentication { get; set; }
        public SpotifyApiCodes? SpotifyCodes { get; set; } = new();


        public SpotifyService(IHttpContextAccessor httpContextAccessor, Interfaces.IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }
        public async Task<T> SendRequest<T>(string endpoint)
        {
            Authentication = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<PkceAuthorization>("User");

            if (Authentication is null)
            {
                throw new InvalidOperationException("User authentication is missing.");
            }

            string accessToken = Authentication.AccessToken!;

            using HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content) ?? throw new NullReferenceException($"{typeof(T).Name} was null after deserialization.");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to retrieve information from Spotify API. Status code: {response.StatusCode}. Error content: {errorContent}");
            }
        }
        public string AuthorizeUser()
        {
            _authorizationService.GenerateCodeChallenge();
            var redirectUrl = _authorizationService.GenerateQueryParams();

            return redirectUrl;
        }
       
        public async Task ExchangeCodeForTokenAsync(string code)
        {
            Authentication = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<PkceAuthorization>("User");
            SpotifyCodes = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<SpotifyApiCodes>("SpotifyApiCodes");

            using var httpClient = new HttpClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", SpotifyApiCodes.ClientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri },
                { "code_verifier", SpotifyCodes!.CodeVerifier! },
            };

            var content = new FormUrlEncodedContent(tokenRequest);

            var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Authentication = JsonSerializer.Deserialize<PkceAuthorization>(responseString);

                if (Authentication is not null)
                {
                    Authentication.Authenticated = true;
                    _httpContextAccessor.HttpContext.Session.SetObjectAsJson("User", Authentication);
                }
            }
        }
    }
}
